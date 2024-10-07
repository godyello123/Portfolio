using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SCommon
{
    public class SCSVReader
    {
        private bool m_IgnoreCase;
        private IFormatProvider m_Provider;
        private List<List<string>> m_Rows;
        private Dictionary<string, int> m_Columns;

        private string TrimQuotes(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            text = text.Replace("&nbsp;", ",");
            return (text[0] == '"' && text.Length > 2) ? text.Substring(1, text.Length - 2).Replace("\"\"", "\"") : text.Replace("\"\"", "\"");
        }

        private void Load(List<string> data, char separator, bool ignoreCase, IFormatProvider provider)
        {
            m_IgnoreCase = ignoreCase;
            m_Provider = provider;
            m_Rows = new List<List<string>>();

            foreach (string text in data)
            {
                int quoteCount = 0;
                int startIndex = 0;
                int endIndex = 0;
                var cols = new List<string>();

                foreach (char c in text)
                {
                    if (c == '"')
                    {
                        quoteCount += 1;
                    }
                    else
                    {
                        if (c == separator && quoteCount % 2 == 0)
                        {
                            cols.Add(TrimQuotes(text.Substring(startIndex, endIndex - startIndex)));
                            startIndex = endIndex + 1;
                        }
                    }
                    endIndex++;
                }

                cols.Add(TrimQuotes(text.Substring(startIndex, endIndex - startIndex)));
                if (string.IsNullOrEmpty(cols[0]) || cols[0][0] != ';') m_Rows.Add(cols);
            }

            int index = 0;
            m_Columns = new Dictionary<string, int>();
            foreach (string text in m_Rows[0]) m_Columns.Add(m_IgnoreCase ? text.ToLower() : text, index++);
        }

        private void Load(TextReader reader, char separator, bool ignoreCase, IFormatProvider provider)
        {
            List<string> data = new List<string>();
            string text = "";
            while (reader.Peek() > -1)
            {
                text += reader.ReadLine();
                if (text == "") continue;

                int quoteCount = 0;
                foreach (char c in text) if (c == '"') quoteCount++;
                if (quoteCount % 2 != 0)
                {
                    text += "\r\n";
                    continue;
                }

                data.Add(text);
                text = "";
            }

            Load(data, separator, ignoreCase, provider);
        }

        public int GetRowCount()
        {
            return m_Rows == null ? 0 : m_Rows.Count - 1;
        }

        public IEnumerable<string> GetColumnList()
        {
            return m_Columns.Keys;
        }

        public int GetColumnCount()
        {
            return m_Columns == null ? 0 : m_Columns.Count;
        }

        public bool DoesColumnExist(string column)
        {
            return m_Columns.ContainsKey(m_IgnoreCase ? column.ToLower() : column);
        }

        public bool Load(string filePath, char separator = ',', bool ignoreCase = true, IFormatProvider provider = null)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                Load(reader, separator, ignoreCase, provider);
            }

            return true;
        }

        public bool Load(string filePath, Encoding encoding, char separator = ',', bool ignoreCase = true, IFormatProvider provider = null)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                StreamReader reader = new StreamReader(fs, encoding);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                Load(reader, separator, ignoreCase, provider);
            }

            return true;
        }

        public bool LoadFromString(string contents, char separator = ',', bool ignoreCase = true, IFormatProvider provider = null)
        {
            using (StringReader reader = new StringReader(contents))
            {
                Load(reader, separator, ignoreCase, provider);
            }

            return true;
        }

        public string GetValue(int row, string column)
        {
            if (column == null) return null;
            if (m_IgnoreCase) column = column.ToLower();
            if (!m_Columns.ContainsKey(column)) return null;
            if (row + 1 >= m_Rows.Count) return null;
            return m_Rows[row + 1][m_Columns[column]];
        }

        public bool TryGetValue(int row, string column, out string value)
        {
            value = GetValue(row, column);
            return !string.IsNullOrEmpty(value);
        }

        public T GetValue<T>(int row, string column)
        {
            try
            {
                return (T)Convert.ChangeType(GetValue(row, column), typeof(T));
            }
            catch(Exception ex)
            {
                //todo : message
                //CommonLogger.Error($"Error parsing value for column '{column}' at row '{row}': {ex.Message}");
                throw;
            }            
        }

        public T GetValue<T>(int row, string column, T def)
        {
            if (!TryGetValue(row, column, out string text)) return def;

            try
            {
                return (T)Convert.ChangeType(text, typeof(T));
            }
            catch (Exception ex)
            {
                //todo : messageBox
                //CommonLogger.Error($"Error parsing value for column '{column}' at row '{row}': {ex.Message}");
                throw;
            }
        }

        public bool TryGetValue<T>(int row, string column, out T value)
        {
            value = default;

            if (!TryGetValue(row, column, out string text)) return false;

            try
            {
                value = (T)Convert.ChangeType(text, typeof(T));
                return true;
            }
            catch (Exception ex)
            {
                //todo : messageBox
                //CommonLogger.Error($"Error parsing value for column '{column}' at row '{row}': {ex.Message}");
                return false;
            }            
        }

        public T GetEnum<T>(int row, string column) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), GetValue(row, column), m_IgnoreCase);
            }
            catch (Exception ex)
            {
                //todo : messageBox
                //CommonLogger.Error($"Error parsing value for column '{column}' at row '{row}': {ex.Message}");
                throw;
            }            
        }

        public T GetEnum<T>(int row, string column, T def) where T : Enum
        {
            if (!TryGetValue(row, column, out string text)) return def;

            try
            {
                return (T)Enum.Parse(typeof(T), text, m_IgnoreCase);
            }
            catch (Exception ex)
            {
                //todo : messageBox
                //CommonLogger.Error($"Error parsing value for column '{column}' at row '{row}': {ex.Message}");
                throw;
            }            
        }

        public bool TryGetEnum<T>(int row, string column, out T value) where T : Enum
        {
            value = default;
            if (!TryGetValue(row, column, out string text)) return false;

            try
            {
                value = (T)Enum.Parse(typeof(T), text, m_IgnoreCase);

                return true;
            }
            catch (Exception ex)
            {
                //todo : messagBox
                //CommonLogger.Error($"Error parsing value for column '{column}' at row '{row}': {ex.Message}");
                return false;
            }
        }
        public bool GetValueBool(int row, string column)
        {
            if (!TryGetValue(row, column, out string text)) return false;

            if (text.ToLower().Equals("true") == true) 
            { 
                return true; 
            }

            return false;
        }
        public System.Numerics.BigInteger GetValueBigInteger(int row, string column)
        {
            if (!TryGetValue(row, column, out string text)) return System.Numerics.BigInteger.Zero;

            try
            {
                return System.Numerics.BigInteger.Parse(text);
            }
            catch
            {
                return System.Numerics.BigInteger.Zero;
            }
        }

        public int GetRowIndex(string column, string value)
        {
            if (column == null) return -1;
            if (m_IgnoreCase) column = column.ToLower();
            if (!m_Columns.ContainsKey(column)) return -1;
            for (int i = 0; i < m_Rows.Count; i++) if (m_Rows[i + 1][m_Columns[column]] == value) return i;
            return -1;
        }

        public List<T> ParseStringToList<T>(string input)
        {
            input = input.Trim('[', ']');
            var elements = input.Split(',');

            var converter = TypeDescriptor.GetConverter(typeof(T));
            var resultList = new List<T>();

            foreach (var element in elements)
            {
                string value = element.Trim(' ', '\"');
                resultList.Add((T)converter.ConvertFromString(value));
            }

            return resultList;
        }

        public List<T> GetList<T>(int row, string column)
        {
            string stringValue = GetValue(row, column);
            if (string.IsNullOrEmpty(stringValue) == true)
                return new List<T>();

            stringValue = stringValue.Trim('[', ']');
            var elements = stringValue.Split(',');

            var converter = TypeDescriptor.GetConverter(typeof(T));
            var resultList = new List<T>();

            foreach (var element in elements)
            {
                string value = element.Trim(' ', '\"');
                resultList.Add((T)converter.ConvertFromString(value));
            }

            return resultList;
        }

        public List<T> GetListEnum<T>(int row, string column) where T : Enum
        {
            string stringValue = GetValue(row, column);
            if (string.IsNullOrEmpty(stringValue) == true)
                return new List<T>();

            stringValue = stringValue.Trim('[', ']');
            var elements = stringValue.Split(',');

            var resultList = new List<T>();

            foreach (var element in elements)
            {
                string value = element.Trim(' ', '\"');
                resultList.Add((T)Enum.Parse(typeof(T), value, m_IgnoreCase));
            }

            return resultList;
        }
        public HashSet<T> GetSet<T>(int row, string column)
        {
            string stringValue = GetValue(row, column);
            if (string.IsNullOrEmpty(stringValue) == true)
                return new HashSet<T>();

            stringValue = stringValue.Trim('[', ']');
            var elements = stringValue.Split(',');

            var converter = TypeDescriptor.GetConverter(typeof(T));
            var resultList = new HashSet<T>();

            foreach (var element in elements)
            {
                string value = element.Trim(' ', '\"');
                resultList.Add((T)converter.ConvertFromString(value));
            }

            return resultList;
        }
        public HashSet<T> GetSetEnum<T>(int row, string column) where T : Enum
        {
            string stringValue = GetValue(row, column);
            if (string.IsNullOrEmpty(stringValue) == true)
                return new HashSet<T>();

            stringValue = stringValue.Trim('[', ']');
            var elements = stringValue.Split(',');

            var resultList = new HashSet<T>();

            foreach (var element in elements)
            {
                //string value = element.Trim(' ', '\"');
                //if (Enum.TryParse(typeof(T), value, out object obj))
                //{
                //    resultList.Add((T)obj);
                //}
            }

            return resultList;
        }
    }
}

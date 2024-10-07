using System;
using System.Collections.Generic;
using System.Drawing;
using Global;

namespace PlayServer
{
	public partial class FormMain
	{
		private CUIMatchData m_UIMatchData = new CUIMatchData();
		private int m_UISelectedUnit = -1;

		public class CUISkillData
		{
			public string Name { get; set; }
			public string Type { get; set; }
			public double CoolTime { get; set; }

			public CUISkillData(string name, bool isAttackSkill, double nextSkillTime)
			{
				Name = name;
				Type = isAttackSkill ? "Attack" : "Defend";

			}
		}

		public class CUIUnitData
		{
			public int UserIdx { get; set; }
			public int UnitIdx { get; set; }
			public int Hp { get; set; }
			public VECTOR3 CurPos { get; set; }
			public VECTOR3 DstPos { get; set; }
			private List<CUISkillData> m_SkillList = new List<CUISkillData>();
			public List<CUISkillData> SkillList { get { return m_SkillList; } }
			//private List<CDefine.ConditionType> m_ConditionList = new List<CDefine.ConditionType>();
			//public List<CDefine.ConditionType> ConditionList { get { return m_ConditionList; } }
		}

		public class CUIMatchData
		{
			public int RemainingSec { get; set; }
			public List<string> MatcherList { get; set; }
			public List<CUIUnitData> UnitDataList { get; set; }
		}

		public void SetUIMatchData(int remainingSec, List<string> matcherList, List<CUIUnitData> unitDataList)
		{
			lock(m_UIMatchData)
			{
				m_UIMatchData.RemainingSec = remainingSec;
				m_UIMatchData.MatcherList = matcherList;
				m_UIMatchData.UnitDataList = unitDataList;
			}
		}

		public void DrawMatch(Graphics graphics)
		{
			lock(m_UIMatchData)
			{
				if(m_UIMatchData.MatcherList == null || m_UIMatchData.UnitDataList == null) return;

				string matcherList = "";
				foreach(var data in m_UIMatchData.MatcherList) matcherList += string.Format("{0}\r\n", data);
				if(textBox_Matcher.Text != matcherList) textBox_Matcher.Text = matcherList;

				graphics.Clear(Color.LightGray);
				graphics.DrawString(m_UIMatchData.RemainingSec.ToString(), DefaultFont, Brushes.Black, 10, 10);

				foreach(var data in m_UIMatchData.UnitDataList)
				{

                    /*
					float radius = data.Record.Radius;
					float height = data.Record.Height;
					Point lt = GameToViewPos(new VECTOR3(data.CurPos.x - radius, data.CurPos.y, data.CurPos.z));
					Point rb = GameToViewPos(new VECTOR3(data.CurPos.x + radius, data.CurPos.y + height, data.CurPos.z));
					graphics.DrawRectangle(Pens.DarkGray, new Rectangle(lt.X, rb.Y, Math.Abs(rb.X - lt.X), Math.Abs(rb.Y - lt.Y)));
                    */

					if(data.UnitIdx == m_UISelectedUnit)
					{
						string info = string.Format("UserIdx : {0}, ", data.UserIdx);
						info += string.Format("UnitIdx : {0}, ", data.UnitIdx);
						//info += string.Format("UnitID : {0}\r\n\r\n", data.Record.ID);
						info += string.Format("Hp : {0}\r\n\r\n", data.Hp);
						info += string.Format("CurPos : [{0:F2}, {1:F2}], ", data.CurPos.x, data.CurPos.y);
						info += string.Format("DstPos : [{0:F2}, {1:F2}]\r\n\r\n", data.DstPos.x, data.DstPos.y);
						info += "Skill\r\n";
						foreach(var skill in data.SkillList) info += string.Format("    {0}({1}) : {2:F2}\r\n", skill.Name, skill.Type, skill.CoolTime);
						info += "\r\nCondition\r\n";
						//foreach(var condition in data.ConditionList) info += string.Format("    {0}\r\n", condition);
						textBox_SelectedUnit.Text = info;
					}
				}
			}
		}
	}
}

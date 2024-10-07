using SCommon;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace SCommon
{
    public class SSegmentPool
    {
        private int _buffSize = 0;
        private int _bucketSize = 0;
        private ConcurrentBag<byte[]> _chunk = new ConcurrentBag<byte[]>();
        private ConcurrentQueue<ArraySegment<byte>> _buckets = new ConcurrentQueue<ArraySegment<byte>>();

        public SSegmentPool(int buffSize, int bucketSize)
        {
            _buffSize = buffSize;
            _bucketSize = bucketSize;

            Create();
        }

        public bool IsValid(int reqSize)
        {
            return (reqSize <= _buffSize);
        }
        public ArraySegment<byte> Rent()
        {
            ArraySegment<byte> retval;
            if (_buckets.TryDequeue(out retval))
                return retval;

            Create();

            if (_buckets.TryDequeue(out retval))
                return retval;
            else
                return new ArraySegment<byte>(new byte[_buffSize]);
        }

        public void Return(ArraySegment<byte> segment)
        {
            _buckets.Enqueue(segment);
        }

        private void Create()
        {
            var buff = new byte[_buffSize * _bucketSize];
            _chunk.Add(buff);

            for (int i = 0; i < _bucketSize; ++i)
            {
                int offset = i * _buffSize;
                var segment = new ArraySegment<byte>(buff, offset, _buffSize);
                _buckets.Enqueue(segment);
            }
        }
    }
    public class SMemoryPool : SSingleton<SMemoryPool>
    {
        private int _defaultPow = 4;
        private List<SSegmentPool> _pool = new List<SSegmentPool>();

        public SMemoryPool()
        {
            int defaultPow = _defaultPow;
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 64000));  // 16
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 32000));  // 32
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 16000));  // 64
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 8000));  // 128
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 4000));  // 256
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 2000));  // 512
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 1000));  // 1024
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 1000));  // 2048
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 1000));  // 4096
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 1000));  // 8192
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 100));   // 16384
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 100));   // 32768
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 100));   // 65536
            _pool.Add(new SSegmentPool((int)Math.Pow(2, (defaultPow++)), 100));   // 131072
        }
        public ArraySegment<byte> Empty { get; } = new ArraySegment<byte>();

        private int IdxFromSize(int size)
        {
            int logarithmVal = (int)(Math.Ceiling(Math.Log(size, 2)));
            int idx = logarithmVal - _defaultPow;
            return idx;
        }
        private SSegmentPool Find(int idx)
        {
            if (idx < 0 || _pool.Count <= idx)
                return null;
            return _pool[idx];
        }

        public ArraySegment<byte> Rent(int size)
        {
            int idx = IdxFromSize(size);
            var hasData = Find(idx);
            if (hasData == null || !hasData.IsValid(size))
                return new ArraySegment<byte>(new byte[size]);

            return hasData.Rent();
        }

        public void Return(ArraySegment<byte> data)
        {
            int idx = IdxFromSize(data.Count);
            var hasData = Find(idx);
            if (hasData != null)
                hasData.Return(data);
        }
    }
}

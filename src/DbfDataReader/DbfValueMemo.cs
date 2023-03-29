using System;
using System.Text;

namespace DbfDataReader
{
    public class DbfValueMemo : DbfValue<byte[]>
    {
        private readonly DbfMemo _memo;

        public DbfValueMemo(int start, int length, DbfMemo memo, Encoding encoding)
            : base(start, length)
        {
            _memo = memo;
            Encoding = encoding;
        }
        
        protected readonly Encoding Encoding;

        public override void Read(ReadOnlySpan<byte> bytes)
        {
            if (Length == 4)
            {
                var startBlock = BitConverter.ToUInt32(bytes);
                Value = _memo?.Get(startBlock);
            }
            else
            {
                var value = Encoding.GetString(bytes);

                if (string.IsNullOrWhiteSpace(value))
                {
                    Value = Array.Empty<byte>();
                }
                else
                {
                    var startBlock = long.Parse(value);
                    Value = _memo?.Get(startBlock);
                }
            }
        }
    }
}
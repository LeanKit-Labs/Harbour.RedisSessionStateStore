using System;
using System.Collections.Generic;
using System.IO;
using System.Web.SessionState;

namespace Harbour.RedisSessionStateStore
{
    internal class RedisSessionState
    {
        public DateTime Created { get; set; }
        public bool Locked { get; set; }
        public int LockId { get; set; }
        public DateTime LockDate { get; set; }
        public int Timeout { get; set; }
        public SessionStateItemCollection Items { get; set; }
        public SessionStateActions Flags { get; set; }

        internal RedisSessionState()
        {
            Items = new SessionStateItemCollection();
            Locked = false;
            Created = DateTime.UtcNow;
        }

        public IDictionary<string, byte[]> ToMap()
        {
            var map = new Dictionary<string, byte[]>()
            {
                { "created", BitConverter.GetBytes(Created.Ticks) },
                { "locked", BitConverter.GetBytes(Locked) },
                { "lockId", Locked ? BitConverter.GetBytes(LockId) : new byte[0] },
                { "lockDate", Locked ? BitConverter.GetBytes(LockDate.Ticks) : new byte[0] },
                { "timeout", BitConverter.GetBytes(Timeout) },
                { "flags", BitConverter.GetBytes((int)Flags) }
            };

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                Items.Serialize(writer);
                map["items"] = ms.ToArray();
                writer.Close();
            }

            return map;
        }

        public static bool TryParse(IDictionary<string, byte[]> raw, out RedisSessionState data)
        {
            if (raw == null || raw.Count != 7)
            {
                data = null;
                return false;
            }

            SessionStateItemCollection sessionItems;

            using (var ms = new MemoryStream(raw["items"]))
            {
                if (ms.Length > 0)
                {
                    using (var reader = new BinaryReader(ms))
                    {
                        sessionItems = SessionStateItemCollection.Deserialize(reader);
                    }
                }
                else
                {
                    sessionItems = new SessionStateItemCollection();
                }
            }

            data = new RedisSessionState()
            {
                Created = new DateTime(BitConverter.ToInt64(raw["created"], 0)),
                Locked = BitConverter.ToBoolean(raw["locked"], 0),
                LockId = raw["lockId"].Length == 0 ? 0 : BitConverter.ToInt32(raw["lockId"], 0),
                LockDate = raw["lockDate"].Length == 0 ? DateTime.MinValue : new DateTime(BitConverter.ToInt64(raw["lockDate"], 0)),
                Timeout = BitConverter.ToInt32(raw["timeout"], 0),
                Flags = (SessionStateActions)BitConverter.ToInt32(raw["flags"], 0),
                Items = sessionItems
            };

            return true;
        }
    }
}

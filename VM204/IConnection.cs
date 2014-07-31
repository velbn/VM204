using System;

namespace VM204
{
    public interface IConnection
    {
        void WriteLine(String packet);
        void Close();
        void Open();
        bool Connected();
    }
}


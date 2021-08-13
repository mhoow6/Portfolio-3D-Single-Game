using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCore
{
    public class FileRoom
    {
        public byte[] file
        {
            get;
            private set;
        }

        public int seek
        {
            get;
            private set;
        }

        public bool Alloc(int fileSize)
        {
            if (file == null)
            {
                file = new byte[fileSize];
                return true;
            }

            if (fileSize != file.Length)
            {
                file = new byte[fileSize];
                return true;
            }

            return false;
        }

        public void AddData(S_FileResponse response)
        {
            Array.Copy(response.file, 0, file, seek, response.file.Length);
            seek += response.file.Length;
        }

        public bool IsFull()
        {
            if (file != null)
                return seek == file.Length ? true : false;
            
            return false;
        }

        public bool Clear()
        {
            if (file != null)
            {
                Array.Clear(file, 0, file.Length);
                seek = 0;
                return true;
            }

            return false;
        }
    }
}

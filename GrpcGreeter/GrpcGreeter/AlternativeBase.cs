using System;

namespace GrpcGreeter
{
    public class UserData
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public List<int> Cellphones { get; set; }
        public UserData(string Name, string LastName, string Address, int CellphoneNumber) 
        { 
            this.Name = Name;
            this.LastName = LastName;
            this.Address = Address;
            this.Cellphones = new List<int>(CellphoneNumber);
        }
    }

    public static class AlternativeBase
    {
        private static Dictionary<int, UserData> Base;
        private static object lockObj = new object();
        public static Dictionary<int, UserData> Instance()
        {
            if (Base == null)
            {
                lock (lockObj)
                {
                    if (Base == null)
                    {
                        Base = new Dictionary<int, UserData>()
                        {
                            { 125, new UserData("Mirko","Bojanic" ,"Unknown", 0631943075) },
                            { 126, new UserData("Bogdan","Micic" ,"Unknown", 0631943084) },
                            { 127, new UserData("Milos","Jovanovic" ,"Unknown", 0631943005) }
                        };
                    }
                }
            }
            return Base;
        }
    }
}

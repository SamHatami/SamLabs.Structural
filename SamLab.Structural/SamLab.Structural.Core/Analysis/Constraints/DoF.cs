namespace SamLab.Structural.Core.Analysis.Constraints
{
    //Degree of freedom
    public class DoF
    {
        public bool Ux { get; set; }
        public bool Uy { get; set; }
        public bool Uz { get; set; }
        public bool Rx { get; set; }
        public bool Ry { get; set; }
        public bool Rz { get; set; }

        public DoF(bool ux, bool uy, bool uz, bool rx, bool ry, bool rz)
        {
            Ux = ux;
            Uy = uy;
            Uz = uz;
            Rx = rx;
            Ry = ry;
            Rz = rz;
        }

        public static DoF CreateFixed()
        {
            return new DoF(true, true, true, true, true, true);
        }

        public static DoF CreateFree()
        {
            return new DoF(false, false, false, false, false, false);
        }

        public static DoF CreatePinned()
        {
            return new DoF(true, true, true, false, false, false);
        }
    }
}
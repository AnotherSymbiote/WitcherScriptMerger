namespace WitcherScriptMerger
{
    public class Range
    {
        public int Start { get; set; }
        public int End { get; set; }

        public int Length
        {
            get { return End - Start; }
            set { End = Start + value; }
        }

        public static Range Empty
        {
            get { return new Range(0, 0); }
        }

        public bool IsEmpty
        {
            get { return Start == 0 && End == 0; }
        }

        public Range()
        {
            Start = End = 0;
        }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public Range(Range rangeToCopy)
        {
            Start = rangeToCopy.Start;
            End = rangeToCopy.End;
        }

        public static Range WithLength(int start, int length)
        {
            return new Range(start, start + length);
        }

        public bool Equals(Range otherRange)
        {
            return (Start == otherRange.Start && End == otherRange.End);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Start, End);
        }
    }
}

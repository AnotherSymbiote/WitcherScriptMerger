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

        public bool Contains(int num)
        {
            return num.IsBetween(Start, End);
        }

        public bool Contains(Range otherRange)
        {
            return (Contains(otherRange.Start) && Contains(otherRange.End));
        }

        public bool OverlapsWith(Range otherRange)
        {
            // Overlapping at only 1 index doesn't count
            // (i.e. start1 == end2 or start2 == end1)
            return Contains(otherRange.Start + 1) || Contains(otherRange.End - 1);
        }

        public bool OverlapsLeftOf(Range otherRange)
        {
            return (OverlapsWith(otherRange) && StartsBefore(otherRange));
        }

        public bool OverlapsRightOf(Range otherRange)
        {
            return (OverlapsWith(otherRange) && EndsAfter(otherRange));
        }

        public bool StartsBefore(Range otherRange)
        {
            return Start < otherRange.Start;
        }

        public bool EndsAfter(Range otherRange)
        {
            return End > otherRange.End;
        }

        public bool Equals(Range otherRange)
        {
            return (Start == otherRange.Start && End == otherRange.End);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, length {2}", Start, End, Length);
        }
    }
}

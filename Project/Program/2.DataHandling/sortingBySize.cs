using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataHandling
{
    public class sortingBySize
    {
        private static class RegexDBO
        {

            /* universalRegex has capture group 1 for non-digits (XXL, UK, US ect)
                 * group 2 for an entire decimal value, and the same for group 5. 
                 * Group 3 and 4 are the values in 2nd group
                 * Group 6 and 7 are the values in 5th group
                 */
            public static string UniversalRegex { get; } = @"^(\D*)\W*((\d*)(\D*?)(\d*))(\D*?)((\d*)(\D*?)(\d*))\D*$";
            /*Checks if the string starts with a letter, followed by one or many Digits
             */
            public static string BraSizesCheck { get; } = @"^([A-z]+)\D*(\d+)$";
            /*The following regex is a combination of multiple. 
             * ^([sS])$|^([mM])$|^([lL])$|^[Xx]+([Ss])$|^[Xx]+([Ll])$|^\d{1,2}[xX]([Ss])$|^\d{1,2}[xX]([lL])$
             * it can be split into the following:
             * caputre group 1. ^([sS])$              //matches any "s"
             * caputre group 2. ^([mM])$              //matches any "m"
             * caputre group 3. ^([lL])$              //matches any "l"
             * caputre group 4. ^[Xx]+([Ss])$         //matches any number of "x" + "s"
             * caputre group 5. ^[Xx]+([Ll])$         //matches any number of "x" + "l"
             * caputre group 6. ^\d{1,2}[xX]([Ss])$   //matches any number between "1-99" + "x" + "s"
             * caputre group 7. ^\d{1,2}[xX]([lL])$   //matches any number between "1-99" + "x" + "l"
             * you could combine it into 3 instead, but it's split up more, so we can assign the size to the correct list.
             * We have a list for each variant of S an L, and then we also have M just as is.
             * each of the individual regex expressions has a capture group, that checks if it ends in S or L. 
             * but since they are combined into one, then the capture group for "number + x + L or S" comes after the one for just S or L.
             * meaning we can check capture group 3, if the size is "l"
             * then we can use the match (capture group 0) and asign that to the list for that size category. 
             */
            public static string TShirtSizeCheck { get; } = @"^([sS])$|^([mM])$|^([lL])$|^[Xx]+([Ss])$|^[Xx]+([Ll])$|^\d{1,2}[xX]([Ss])$|^\d{1,2}[xX]([lL])$";
            public static string TshirtSizeLMS { get; } = @"[^mMsSlL]*([mMsSlL]){0,1}";
            public static string TshirtSizeAmountOfExtra { get; } = @"^(\d+)[xX]|^([xX]+)";
            public static string TShirtToNumberCheck { get; } = @"^(\d+)\W*([\d*xX]*[sSmMlL])$|^([\d*xX]*[sSmMlL])\W*(\d+)$";
            public static string TshirtSizePlusRangeCheck { get; } = @"^([\dxX]*)([mMsSlL]{1})\D+?([\dxX]*)([mMsSlL]){1}(\d)*";
            public static string Words { get; } = @"[A-z]+";
        }
        /// <summary>
        /// Sorts multiple types of clothing articles from 1 method
        /// </summary>
        /// <param name="listOfSizes"></param>
        /// <param name="sortByFirstCheck"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<CanonicalModel> SmartSorter(List<CanonicalModel> listOfSizes, bool sortByFirstCheck = true)
        {
            //We do some setup that's common for each item
            var sortList = new List<CanonicalModel>();
            int sortByFirst = sortByFirstCheck ? 10000 : 1;
            int sortBySecond = sortByFirstCheck ? 1 : 10000;

            bool isABraSizeFix = false;

            foreach (CanonicalModel CM in listOfSizes)
            {
                string item = CM.Size;
                var regMatch = Regex.Match(item, RegexDBO.UniversalRegex);
                //Since EU 45 would trigger brasize, we run this for each element. (collapsible with the region to clean it up a bit)
                #region BraSizeFix
                if (Regex.IsMatch(item, RegexDBO.BraSizesCheck))
                {
                    var braLetterCheck = Regex.Match(item, RegexDBO.BraSizesCheck).Groups[1].Value.ToUpper().ToCharArray();
                    isABraSizeFix = true; //assume it's a brasize first
                    if (braLetterCheck.Count() > 1)
                    {
                        //set the first char in the array
                        char firstChar = braLetterCheck[0];
                        //start foreach loop on 2nd value (Index 1)
                        for (int i = 1; i < braLetterCheck.Count(); i++)
                        {
                            //if the letters aren't the same, then it's not a braSize
                            if (braLetterCheck[i] != firstChar)
                            {
                                isABraSizeFix = false;
                                break;
                            }
                        }
                    }
                }
                #endregion

                //"Algorith" starts here ↓
                if (Regex.IsMatch(regMatch.Groups[0].Value, RegexDBO.TShirtSizeCheck))//tShirtSize
                {

                    /* S M AND L
                     * we dont need to inlcude S M or L in the regex as a seperate group, as we can find it in code easier
                     * Then we set a number according to if it's S M or L. (1,2 and 3 respectivly)
                     * then we multiply by a high number to make a big valley of numbers between them, that we can put extra numbers in.
                     * We start with an asumption of what it is. Doens't matter what. Here we start with asuming it's a S size (just to cut an else statement away)
                     * We set the IndexValue to 1. The lowest of the 3 possibilities.
                     * (all of this is needed, as the order in the char-int is wrong. otherwise we could use the char number, 76(L) 77(M) and 83(S)) 
                     * (s should be smallest, and L largest for this ↑ to work)
                     * As 5xl is larger than l, but 5xs is smaller than s. we also set the var SorL as -1 or 1. 
                     * So when we add the number, we either subtract or add to the big valley of numbers.
                     * (ie. 5xl = l(3)*1000 = 30.000. 5*SorL(1) = 5. 30.000 + 5 = 30.005)
                     * (ie. 5xs = s(1)*1000 = 10.000. 5*SorL(-1) = -5. 10.000 +(-5) = 9.995)
                     */

                    //breaking up the TshirtSize into it's amount of "extra", and it's identifier (Large,Medium or Small)(LMorS)
                    var amountOfExtra = Regex.Match(item, RegexDBO.TshirtSizeAmountOfExtra);
                    var LMorS = Regex.Match(item, RegexDBO.TshirtSizeLMS);

                    var LMorSLetter = LMorS.Groups[1].Value.ToUpper().ToCharArray()[0];
                    int IndexValue = 1;
                    int SorL = -1;
                    if (LMorSLetter == 'L')
                    {
                        IndexValue = 3;
                        SorL = 1;
                    }
                    else if (LMorSLetter == 'M')
                        IndexValue = 2;

                    //The "*10000" could've been "sortByFirst", but it can be set to 1, so the valley would disapear and ruin the sorting. 
                    if (!string.IsNullOrEmpty(amountOfExtra.Groups[1].Value))
                    {
                        //nX                                            ↓<3>                                   ↓<4>         ↓<1>     ↓<2>
                        CM.SortingIndex = IndexValue * 10000 + (double.Parse(amountOfExtra.Groups[1].Value) * 1000 + (-1 * SorL)) * SorL;
                        sortList.Add(CM);
                    }
                    //                                              ↑ S M or L                        
                    else if (!string.IsNullOrEmpty(amountOfExtra.Groups[2].Value))  //X
                    {
                        CM.SortingIndex = IndexValue * 10000 + amountOfExtra.Groups[2].Value.Length * 1000 * SorL;
                        sortList.Add(CM);
                    }

                    else                                                            //S,M or L
                    {
                        CM.SortingIndex = IndexValue * 10000;
                        sortList.Add(CM);
                    }
                    /* <1>
                     * This segment is meant to prioritize numbers before raw X's. so 3X is shown before xxx regardless of it's Large or Small.
                     * So if it's Large, we "value + (-1 * 1)" = -1 to the value, so it's seen as "smaller" than XXX
                     * And if it's small, it adds 1 to the value, so it becomes one larger, so when we flip the value with <2>, then it's also 1 "smaller"
                     * <2>
                     * We "flip" the value if it's an Small, so that the "biggest" number, like 99XS becomes "-99"xs, and is seen as "smaller" in the 
                     * sorting algorithm, and will place it before a smaller value
                     * and if it's a Large, we dont flip it, and it will just sort smaller first.
                     * So <2> is to flip the priority of the sorting algorithm,
                     * from "largest" first in the case of "S"
                     * to "largest" last, in the case of "L"
                     * <3>
                     * Parse the value to a double, as it's 5 in "5xl"
                     * <4>
                     * add 1.000 to the value, so the +-1 difference in 3xl and xxxl made in <1> 
                     * doesn't change it's position relative to the other values. So 3xl is still considered larger than xxl, even though we -1 from it's value
                     * (xxl = 32.000) (3xl = 32.999) (xxxl = 33.000)
                     */
                }
                else if (Regex.IsMatch(regMatch.Groups[0].Value, RegexDBO.TshirtSizePlusRangeCheck)) // if it's a tshirtSizeRange. ie. L-2xl
                {
                    /*
                     * if it's sort by first, then the Index is the 1st value as the big valley, and the 2nd value in the valley.
                     * if it's sort by second, then the Index is the 2st value as the big valley, and the 1nd value in the valley
                     */


                    //getting the match to work with. Group 0 is the entire string.
                    var comparison = Regex.Match(regMatch.Groups[0].Value, RegexDBO.TshirtSizePlusRangeCheck);

                    //setup for the 1st value                   Group 0 here is the amount of extra in the first value
                    var amountOfExtraFirst = Regex.Match(comparison.Groups[0].Value, RegexDBO.TshirtSizeAmountOfExtra);
                    var LMorSFirst = comparison.Groups[2].Value;
                    var LMorSValueFirst = LMorSFirst.ToUpper().ToCharArray()[0];
                    int firstIndexValue = 1;
                    int SorLFirst = -1;

                    //setup for the 2nd value                   Group 3 here is the amount of extra in the second value
                    var amountOfExtraSecond = Regex.Match(comparison.Groups[3].Value, RegexDBO.TshirtSizeAmountOfExtra);
                    var LMorSSecond = comparison.Groups[4].Value;
                    var LMorSValueSecond = LMorSSecond.ToUpper().ToCharArray()[0];
                    int secondIndexValue = 1;
                    int SorLSecond = -1;

                    //Translating the LetterValues to a Number value
                    #region FindingLetterValues
                    if (LMorSValueFirst == 'L')
                    {
                        firstIndexValue = 3;
                        SorLFirst = 1;
                    }
                    else if (LMorSValueFirst == 'M')
                        firstIndexValue = 2;

                    if (LMorSValueSecond == 'L')
                    {
                        secondIndexValue = 3;
                        SorLSecond = 1;
                    }
                    else if (LMorSValueSecond == 'M')
                        secondIndexValue = 2;
                    #endregion


                    //if sortByFirst, calculate the 2nd values first to add to the first one
                    if (sortByFirstCheck)
                    {
                        double secondValue = secondIndexValue;
                        if (!string.IsNullOrEmpty(amountOfExtraSecond.Groups[1].Value)) //nX
                        {
                            secondValue = secondIndexValue + ((double.Parse(amountOfExtraSecond.Groups[1].Value) + (-1 * SorLSecond)) * SorLSecond);
                        }
                        else if (!string.IsNullOrEmpty(amountOfExtraSecond.Groups[2].Value)) //X
                        {
                            secondValue = secondIndexValue + amountOfExtraSecond.Groups[2].Value.Length * SorLSecond;
                        }


                        //then Add the 2nd values to the 1st values
                        //this bit is more or less the same as the pure Tshirt one, so im not going to explain it again here.
                        //main difference is we just add the second Value on the end as it is. No big numbers going on there
                        if (!string.IsNullOrEmpty(amountOfExtraFirst.Groups[1].Value)) //nX
                        {
                            CM.SortingIndex = firstIndexValue * 10000 + ((double.Parse(amountOfExtraFirst.Groups[1].Value) + (-1 * SorLFirst)) * 1000) * SorLFirst + secondValue;
                            sortList.Add(CM);
                        }
                        else if (!string.IsNullOrEmpty(amountOfExtraFirst.Groups[2].Value)) //X
                        {
                            CM.SortingIndex = firstIndexValue * 10000 + ((amountOfExtraFirst.Groups[2].Value.Length * SorLFirst) * 1000) + secondValue;
                            sortList.Add(CM);
                        }
                        else //S,M or L
                        {
                            CM.SortingIndex = firstIndexValue * 10000 + secondValue;
                            sortList.Add(CM);
                        }
                    }
                    else
                    {
                        //else, calculate 1st value first and add to second value
                        double firstValue = firstIndexValue;
                        if (!string.IsNullOrEmpty(amountOfExtraFirst.Groups[1].Value)) //nX
                        {
                            firstValue = firstIndexValue + ((double.Parse(amountOfExtraFirst.Groups[1].Value) + (-1 * SorLFirst)) * SorLFirst);
                        }
                        else if (!string.IsNullOrEmpty(amountOfExtraFirst.Groups[2].Value)) //X
                        {
                            firstValue = firstIndexValue + amountOfExtraFirst.Groups[2].Value.Length * SorLFirst;
                        }


                        //then add 1st values to sortByLast

                        if (!string.IsNullOrEmpty(amountOfExtraSecond.Groups[1].Value)) //nX
                        {
                            CM.SortingIndex = secondIndexValue * 10000 + (double.Parse(amountOfExtraSecond.Groups[1].Value) + (-1 * SorLSecond) * 1000) * SorLSecond + firstValue;
                            sortList.Add(CM);
                        }
                        else if (!string.IsNullOrEmpty(amountOfExtraSecond.Groups[2].Value)) //X
                        {
                            CM.SortingIndex = secondIndexValue * 10000 + ((amountOfExtraSecond.Groups[2].Value.Length * SorLSecond) * 1000) + firstValue;
                            sortList.Add(CM);
                        }
                        else //S,M or L
                        {
                            CM.SortingIndex = secondIndexValue * 10000 + firstValue;
                            sortList.Add(CM);
                        }
                    }
                }
                else if (Regex.IsMatch(regMatch.Groups[0].Value, RegexDBO.TShirtToNumberCheck))
                {
                    //tshirt size with 1 or 2 numbers. (L32 or 5XS17)

                    var tShirtToNumberRegex = Regex.Match(item, RegexDBO.TShirtToNumberCheck);
                    double numberValue = 0;
                    string wordsValue = "";
                    if (Regex.IsMatch(tShirtToNumberRegex.Groups[3].Value, RegexDBO.Words))
                    {
                        numberValue = double.Parse(tShirtToNumberRegex.Groups[4].Value);
                        wordsValue = tShirtToNumberRegex.Groups[3].Value;
                    }
                    else
                    {
                        numberValue = double.Parse(tShirtToNumberRegex.Groups[1].Value);
                        wordsValue = tShirtToNumberRegex.Groups[2].Value;

                    }

                    var amountOfExtra = Regex.Match(wordsValue, RegexDBO.TshirtSizeAmountOfExtra);
                    var LMorS = Regex.Match(wordsValue, RegexDBO.TshirtSizeLMS);
                    var LMorSValue = LMorS.Groups[1].Value.ToUpper().ToCharArray()[0];
                    int IndexValue = 1;
                    double SorL = -1;

                    if (LMorSValue == 'L')
                    {
                        IndexValue = 3;
                        SorL = 1;
                    }
                    else if (LMorSValue == 'M')
                        IndexValue = 2;

                    //default is that it's just S M or L
                    double tShirtSizeValue = IndexValue * sortByFirst;

                    //Here sortByFirst and SortBySecond should work without any issues

                    if (!string.IsNullOrEmpty(amountOfExtra.Groups[1].Value)) //nX
                    {
                        tShirtSizeValue = (IndexValue * sortByFirst) + ((double.Parse(amountOfExtra.Groups[1].Value) * sortByFirst / 10 + (-1 * SorL)) * SorL);
                    }
                    else if (!string.IsNullOrEmpty(amountOfExtra.Groups[2].Value)) //X
                    {
                        tShirtSizeValue = (IndexValue * sortByFirst) + (amountOfExtra.Groups[2].Value.Length * sortByFirst / 10) * SorL;
                    }

                    CM.SortingIndex = tShirtSizeValue + numberValue * sortBySecond;
                    sortList.Add(CM);
                }
                else if (isABraSizeFix)
                //BraSizes
                {
                    var braRegex = Regex.Match(item, RegexDBO.BraSizesCheck);
                    var braLetter = braRegex.Groups[1].Value.ToString().ToUpper().ToCharArray();
                    bool isABraSize = true;
                    int HigherSize = braLetter.Count() - 1;
                    if (braLetter.Count() > 1)
                    {
                        char firstChar = braLetter[0];
                        foreach (char currentChar in braLetter)
                        {
                            if (currentChar != firstChar)
                            {
                                isABraSize = false;
                                break;
                            }
                            if (currentChar == 'A')
                            {
                                HigherSize = -braLetter.Count();
                                //AA is smaller than A, so we get the negative amount of entries, and add that to the size later
                            }
                        }
                    }
                    if (isABraSize)
                    { //                                   ↓<1>                                             ↓<2>
                        CM.SortingIndex = ((int)braLetter[0] + HigherSize) * sortByFirst + int.Parse(braRegex.Groups[2].Value) * sortBySecond;
                        sortList.Add(CM);
                    }
                    /*
                     * <1>
                     * since standards are different, someone could use A,B,C,D,E,F,G...
                     * While other will use "A,BB,D,DD"... something like that. Where a duplicate letter is one higher than the previous one
                     * (expect AA, then it's smaller than A)
                     * And worst case scenario is, that they mix. So it could maybe go like "A,B,CC,D" 
                     * where CC and D "should" be the same size, so by just adding the amount of extra characters, we get that effect.
                     * (if it's only 1 value, it's just adding 0)
                     * <2>
                     * if SortBySecond is applied, then it will sort by the numbers first, and the Cup size after
                     */

                }
                else //any leftover will get sorted with the universal regex. it pretty much only cares about the numbers.
                {
                    //if there's a string, but no number, then we just add it to the end of the sorting. Works with Onesize, and also with anything not acounted for ¯\_(ツ)_/¯
                    if (!string.IsNullOrEmpty(regMatch.Groups[1].Value) && string.IsNullOrEmpty(regMatch.Groups[3].Value))
                    {
                        CM.SortingIndex = 10000000;
                        sortList.Add(CM); //onesize or other stray word without numbers, we'll place at the end.
                    }
                    //---------------------------------------------------
                    // 1 number
                    if (!string.IsNullOrEmpty(regMatch.Groups[3].Value) //first number
                        && string.IsNullOrEmpty(regMatch.Groups[7].Value)) //if 7 is unset, then there's only 1 number in the searchstring
                    {
                        CM.SortingIndex = double.Parse(regMatch.Groups[3].Value) * sortByFirst;
                        sortList.Add(CM); //we still add the 1k so it'll play well if there's mixed values
                    }
                    //---------------------------------------------------
                    // 2 numbers
                    else if (!string.IsNullOrEmpty(regMatch.Groups[3].Value) //first number
                        && !string.IsNullOrEmpty(regMatch.Groups[10].Value) // after decimal point
                        && string.IsNullOrEmpty(regMatch.Groups[8].Value)) // if this is set, then there's 3 or more numbers
                    {
                        double firstNumber = double.Parse(regMatch.Groups[3].Value);
                        double secondNumber = double.Parse(regMatch.Groups[10].Value);
                        var WordsInItem = Regex.Matches(regMatch.Groups[0].Value, RegexDBO.Words);
                        if (WordsInItem.Count > 1)
                        {
                            if (Regex.IsMatch(WordsInItem[1].Groups[0].Value, "y", RegexOptions.IgnoreCase))
                            {
                                secondNumber *= 10;
                            }
                        }
                        if (Regex.IsMatch(regMatch.Groups[9].Value, @"\,|\.")) //if it's a decimal
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value + "." + regMatch.Groups[10].Value) * sortByFirst;
                            sortList.Add(CM);
                        }
                        else if (Regex.IsMatch(regMatch.Groups[9].Value, @"\\|\/|\-|\s*?")) //two ints. example: 27 / 12 or 13 - 21
                        {
                            CM.SortingIndex = firstNumber * sortByFirst + secondNumber * sortBySecond;
                            sortList.Add(CM);
                        }
                        else
                            throw new Exception("Error with 2 numbers: couldn't find endpoint");
                        //dont know if these exceptions are needed. they were usefull for debugging
                    }
                    //---------------------------------------------------
                    // 3 numbers
                    else if (!string.IsNullOrEmpty(regMatch.Groups[3].Value) //first number
                        && !string.IsNullOrEmpty(regMatch.Groups[8].Value) //second number
                        && !string.IsNullOrEmpty(regMatch.Groups[10].Value) //third number
                        && string.IsNullOrEmpty(regMatch.Groups[5].Value)) //checks if 4th number is set
                    {
                        if (Regex.IsMatch(regMatch.Groups[6].Value, @"\.|\,|\/")) //if the 1nd special character is a decimal, then combine the 1nd and 2rd number as the 1st value
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value + "." + regMatch.Groups[8].Value) * sortByFirst + double.Parse(regMatch.Groups[10].Value) * sortBySecond;
                            sortList.Add(CM);

                        }
                        else if (Regex.IsMatch(regMatch.Groups[9].Value, @"[\.\,|\/]")) //if the 1st special char isn't a decimal point, then if 2nd one is
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value) * sortByFirst + double.Parse(regMatch.Groups[8].Value + "." + regMatch.Groups[10].Value) * sortBySecond;
                            sortList.Add(CM);

                        }
                        else if (Regex.IsMatch(regMatch.Groups[6].Value, @"\\|\/")) //if 1st special char is seperate value
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value) * sortByFirst + double.Parse(regMatch.Groups[8].Value) * sortBySecond;
                            sortList.Add(CM);

                        }
                        else if (Regex.IsMatch(regMatch.Groups[6].Value, @"\-"))
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value) * sortByFirst + double.Parse(regMatch.Groups[10].Value) * sortBySecond;
                            sortList.Add(CM);
                        }
                        else
                            throw new Exception("Error with 3 numbers: couldn't find endpoint");
                    }

                    //---------------------------------------------------
                    // 4 numbers
                    else if (!string.IsNullOrEmpty(regMatch.Groups[5].Value)) //if group 5 is set, then there's 4 numbers
                    {
                        //If it's 2 decimals, then we parse them as such
                        if (Regex.IsMatch(regMatch.Groups[4].Value, @"\,|\.") && Regex.IsMatch(regMatch.Groups[9].Value, @"\,|\."))
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value + "." + regMatch.Groups[5].Value) * sortByFirst + double.Parse(regMatch.Groups[8].Value + "." + regMatch.Groups[10].Value) * sortBySecond;
                            sortList.Add(CM);
                        }
                        //if it's 2 ranges, we only look at the first number in the ranges to sort.
                        else if (Regex.IsMatch(regMatch.Groups[4].Value, @"\\|\/|-"))
                        {
                            CM.SortingIndex = double.Parse(regMatch.Groups[3].Value) * sortByFirst + double.Parse(regMatch.Groups[8].Value) * sortBySecond;
                            sortList.Add(CM);
                        }
                        else
                            throw new Exception("Error with 4 numbers: couldn't find endpoint");
                    }
                }

            }
            //Then at the end, we sort the entire list with the same sorting algorithm. Changing this part could speed up this process
            //if you have a large amount of sizes, you are trying to sort at once. (this *should* be the fastest with up to 30 items)
            InsersionSortByIndex(ref sortList);

            return sortList;
        }



        /// <summary>
        /// The Actual sorting algorithm
        /// </summary>
        /// <param name="sortList"></param>
        private static void InsersionSortByIndex(ref List<CanonicalModel> sortList)
        {
            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int i = 1; i < sortList.Count; i++)
                {
                    if (sortList[i].SortingIndex < sortList[i - 1].SortingIndex)
                    {
                        var temp = sortList[i - 1];
                        sortList[i - 1] = sortList[i];
                        sortList[i] = temp;

                        sorted = false;
                        //if we swap once, we restart the loop
                        break;
                    }
                }
            }
        }
    }
}

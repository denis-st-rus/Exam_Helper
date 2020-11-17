﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Exam_Helper.TestMethods
{
    public class TestPuzzle
    {
        public string Thereom { get; set; }

        private string[] words;
        private int[] right_index_order;
        string[] test_strings;
        private string[] blocks;

        private int words_in_block;
        private int blocks_amount;
        private float percent;
        public bool isDiffLenghtOfBlocks;
        public bool isSetBlocksByDefault;
        public int separatingIndex;
        private bool isPossible;

        private const float PERCENT = 33f;
        private const int MAX_WORDS_IN_BLOCK = 10;
        private const int MIN_WORDS_IN_BLOCK = 2;
        private const int MIN_BLOCKS = 3;


        public bool IsSuccessed
        {
            get
            {
                return isPossible;
            }
        }
        public string[] Words
        {
            get
            {
                return words;
            }
        }
        public int[] RightIndexes
        {
            get
            {
                return right_index_order;
            }
        } //
        public string[] TestStrings
        {
            get
            {
                return test_strings;
            }
        } //

        public TestPuzzle(string Thereom, string Instruction = "33;false;false;0;")
        {
            /*
             Instruction:
            [0] - percent                       [1; 100]            by default 33
            [1] - Different lenght of blocks    [true,false]        by default false
            [2] - Set blocks by default         [true,false]        by default false
            [3] - Separating                    [0; 2]              by default 0
                    0 - sepIndulge, 1 - sepByParts, 2 - sepBySentances
             */

            PuzzleInstruction puzzleInstruction = new PuzzleInstruction(Instruction);

            percent = puzzleInstruction.percent;
            isDiffLenghtOfBlocks = puzzleInstruction.isDiffLenghtOfBlocks;
            isSetBlocksByDefault = puzzleInstruction.isSetBlocksByDefault;
            separatingIndex = puzzleInstruction.separatingIndex;

            if (string.IsNullOrEmpty(Thereom))
                throw new Exception("incorrect string");

            this.Thereom = Thereom;
            percent = (101 - percent) / 100;


            words = Thereom.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            isPossible = CreateTest();
        }

        private bool CreateTest()
        {
            //Counting words in a block for [isDiffLenghtOfBlocks=false]
            words_in_block = 
                percent * MAX_WORDS_IN_BLOCK < MIN_WORDS_IN_BLOCK ?
                MIN_WORDS_IN_BLOCK :
                (int)(percent * MAX_WORDS_IN_BLOCK);
            //Counting blocks for [separatingIndex=0]
            blocks_amount = 
                words.Length / words_in_block + 
                (words.Length % words_in_block == 0 ? 0 : 1);

            //Checking an ambit for [block_amount]
            if (blocks_amount < MIN_BLOCKS) blocks_amount = MIN_BLOCKS;

            //Creating arrays
            right_index_order = new int[blocks_amount];
            test_strings = new string[blocks_amount];
            blocks = new string[blocks_amount];

            //One more ambit                        -editable
            if (words.Length < 6) return false;
            
            Random rnd = new Random();

            //Setting blocks_amount for [isDiffLenghtOfBlocks]
            if (isDiffLenghtOfBlocks)
            {
                //Amout words in every block
                int[] temp = new int[blocks_amount];
                int amount_used_words = 0;
                for(int i = 0; i< blocks_amount; i++)
                {
                    //[+1; -1]
                    temp[i] = rnd.Next(words_in_block - 1, words_in_block + 2);
                    amount_used_words += temp[i];
                }
                var diff = words.Length - amount_used_words;

                while (diff > 0)
                {
                    temp[rnd.Next() % blocks_amount]++;
                    diff--; 
                }

                while (diff < 0)
                {
                    var buf = rnd.Next() % blocks_amount;
                    if (temp[buf] > MIN_WORDS_IN_BLOCK)
                    {
                        temp[buf]--;
                        diff++;
                    }
                }

                //Filling blocks
                int curr_word = 0;
                int curr_block = 0;
                foreach(var amount in temp)
                {
                    blocks[curr_block] = "";
                    for (int i = 0; i < amount; i++)
                    {
                        blocks[curr_block] += words[curr_word++] + " ";
                    }
                    curr_block++;
                }

            }
            else
            {
                int i = 0;
                int curr_words_amount = 0;
                foreach (var word in words)
                {
                    if (curr_words_amount == words_in_block)
                    {
                        i++;
                        blocks[i] = "";
                        curr_words_amount = 0;
                    }
                    blocks[i] += word + " ";
                    curr_words_amount++;
                }
            }
            
            
            
            for (int j = 0; j < blocks_amount; ++j)
            {
                right_index_order[j] = -1;
                test_strings[j] = null;
            }
            for (int j = 0; j < blocks_amount; ++j)
            {
                int rm = rnd.Next() % blocks_amount;
                while (test_strings[rm] != null) rm = rnd.Next() % blocks_amount;
                right_index_order[j] = rm;
                test_strings[rm] = blocks[j];
            }

            return true;
        }
    }

    class PuzzleInstruction
    {
        /*
             Instruction:
            [0] - percent                       [1; 100]            by default 33
            [1] - Different lenght of blocks    [true,false]        by default false
            [2] - Set blocks by default         [true,false]        by default false
            [3] - Separating                    [0; 2]              by default 0
                    0 - sepIndulge, 1 - sepByParts, 2 - sepBySentances
             */

        public float percent;
        public bool isDiffLenghtOfBlocks;
        public bool isSetBlocksByDefault;
        public int separatingIndex;

        private const float PERCENT = 33f;
        private const bool IS_DIFF_LENGHT_OF_BLOCKS = false;
        private const bool IS_SET_BLOCKS_BY_DEFAULT = false;
        private const int SEPARATING_INDEX = 0;

        public PuzzleInstruction(string instruction)
        {
            string[] instructions = instruction.Split(";");

            if (!float.TryParse(instructions[0], out percent)) percent = PERCENT;

            if (!bool.TryParse(instructions[1], out isDiffLenghtOfBlocks)) isDiffLenghtOfBlocks = IS_DIFF_LENGHT_OF_BLOCKS;

            if (!bool.TryParse(instructions[2], out isSetBlocksByDefault)) isSetBlocksByDefault = IS_SET_BLOCKS_BY_DEFAULT;

            if (!int.TryParse(instructions[3], out separatingIndex)) separatingIndex = SEPARATING_INDEX;

            if (percent < 1 || percent > 100)
                throw new Exception("incorrect percent");

            if (separatingIndex < 0 || separatingIndex > 2)
                throw new Exception("Out of range: separatingIndexs");
        }
    }
}

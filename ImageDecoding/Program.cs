using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageDecoding
{
    /*
     * You have been placed in charge of your company’s archive of digital data, and your first job is to convert a lot of old pictures into a newer format. But you have no software to read the old images,
     * so you need to write it.

You do have a description of the old image format. The old images are 1-bit images (each pixel is either on or off) and are in a simple compressed format. 
    The compression is a form of run-length encoding, which is popular because it runs efficiently on older hardware.

For each encoded image, produce a rendered version so that you can visually inspect it. You should also detect errors in the original encoded image, if there are any.

Input
Input consists of a sequence of up to  images. Each image starts with a line containing an integer  indicating the number of scanlines in the image. The following  lines each contain one scanline. 
    Each scanline starts with either ‘.’ or ‘#’, indicating the value of the first pixel on the scanline. Following this are up to  integers each in the range  indicating the lengths of each subsequent run of pixels. 
    Each scanline has at most  total pixels (the sum of the integers on the line). Each run uses only one pixel value, which alternates between ‘.’ and ‘#’ with each run. Input ends with a line containing just the number .

Output
For each image, decode and output image according to run-length encoding. In other words, for each scanline, expand each run of pixels of length  into  copies of that pixel value.

Some images may not have the same number of pixels output for all decoded scanlines. This is an error condition, which your program should identify by producing a line after the image with the text ‘Error decoding image’.

Separate adjacent images with a blank line.
     * 
     * https://open.kattis.com/problems/imagedecoding
     * 
     * */
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            bool decoding = false;
            int numberOfLinesToWrite = 0;
            int currentLine = 0;
            bool isFirstImage = true;
            bool error = false;

            while ((line = Console.ReadLine()) != null)
            {
                if (isFirstImage == false && decoding == true && currentLine == 0)
                {
                    Console.WriteLine();
                }

                if (int.TryParse(line, out int lines))
                {
                    decoding = true;
                    numberOfLinesToWrite = lines;
                    if (lines == 0 || lines > 100)
                    {
                        break;
                    }
                }
                else if (line.StartsWith("#") && decoding)
                {
                    int written = ImageDecoder.Decode(line, false, (currentLine == 0));
                    if (written > ImageDecoder.PixelsOnFirstLine)
                    {
                        error = true;
                    }
                    currentLine++;
                }
                else if (line.StartsWith(".") && decoding)
                {
                    int written = ImageDecoder.Decode(line, true, (currentLine == 0));
                    if(written > ImageDecoder.PixelsOnFirstLine)
                    {
                        error = true;
                    }
                    currentLine++;
                }

                if (decoding && currentLine == numberOfLinesToWrite)
                {
                    if (error)
                    {
                        Console.WriteLine("Error decoding image");
                    }
                    decoding = false;
                    numberOfLinesToWrite = 0;
                    currentLine = 0;
                    isFirstImage = false;
                    ImageDecoder.PixelsOnFirstLine = 0;
                    error = false;
                }
            }
        }

        static class ImageDecoder
        {
            public static int PixelsOnFirstLine { get; set; } = 0;
            public static int Decode(string data, bool useDotPixelAsFirstCharacter, bool isFirstLine)
            {
                string[] splitData = data.Split(" ");
                char firstChar = '#';
                char secondChar = '.';
                int charactersWritten = 0;

                if (isFirstLine)
                {
                    for (int i = 1; i < splitData.Length; i++)
                    {
                        PixelsOnFirstLine += int.Parse(splitData[i]);
                    }
                }

                if (useDotPixelAsFirstCharacter)
                {
                    firstChar = '.';
                    secondChar = '#';
                }

                bool writeFirstChar = true;
                for (int i = 1; i < splitData.Length; i++)
                {
                    if (int.TryParse(splitData[i], out int numberOfCharactersToWrite))
                    {
                        for (int y = 0; y < numberOfCharactersToWrite; y++)
                        {
                            if (writeFirstChar)
                            {
                                Console.Write(firstChar);
                                charactersWritten++;
                            }
                            else
                            {
                                Console.Write(secondChar);
                                charactersWritten++;
                            }
                        }
                        writeFirstChar = (i % 2 == 0);
                    }
                }

                Console.WriteLine();
                return charactersWritten;
            }
        }
    }
}
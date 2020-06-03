using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cards.Tests
{
    [TestClass()]
    public class QuizTests
    {

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfCardsRangeException))]
        public void LastCardTest()
        {
            // arrange
            Quiz quiz = new Quiz("");            
            quiz.AddNewCard();
            // act
            quiz.NextCard();
        }

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfCardsRangeException))]
        public void FirstCardTest()
        {
            // arrange
            Quiz quiz = new Quiz("");
            quiz.AddNewCard();
            // act
            quiz.PreviousCard();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ImportInvalidPathTest()
        {
            // arrange
            Quiz quiz = new Quiz("");
            // act
            quiz.Import();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidCardsException))]
        public void SaveWithEmptyCardTest()
        {
            // arrange
            Quiz quiz = new Quiz("test");
            quiz.AddNewCard();
            // act
            quiz.Save();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Cards
{
    /// <summary>
    /// Karta s otázkou a odpovědí.
    /// </summary>
    [Serializable()]
    public struct Card
    {
        /// <summary>
        /// Otázka.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Odpověď.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Vytvoří novou instanci karty.
        /// </summary>
        /// <param name="question">Otázka</param>
        /// <param name="answer">Odpověď</param>
        public Card(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
    
    /// <summary>
    /// Kvíz vytvořen z kolekce karet.
    /// </summary>
    public class Quiz : INotifyPropertyChanged
    {
        private List<Card> _cards = new List<Card>(); // kolekce karet
        private int _currentCardIndex = 0; // index právě prohlížené karty            
        private string _path;   // cesta k souboru kvízu
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Vrací otázku právě prohlížené karty.
        /// </summary>
        public string CurrentCardQuestion
        {
            get
            {
                return _cards[_currentCardIndex].Question;
            }
            set
            {
                string tmpAnswer = _cards[_currentCardIndex].Answer;
                _cards[_currentCardIndex] = new Card(value, tmpAnswer);
            }
        }

        /// <summary>
        /// Vrací odpověď právě prohlížené karty.
        /// </summary>
        public string CurrentCardAnswer
        {
            get
            {
                return _cards[_currentCardIndex].Answer;
            }
            set
            {
                string tmpQuestion = _cards[_currentCardIndex].Question;
                _cards[_currentCardIndex] = new Card(tmpQuestion, value);
            }
        }        

        /// <summary>
        /// Vytvoření instance kvízu.
        /// </summary>
        /// <param name="path">Cesta k souboru kvízu</param>
        public Quiz(string path)
        {
            _path = path;     
        }        

        /// <summary>
        /// Změní právě prohlíženou kartu na následující v kolekci. Vyvolá výjimku, pokud je karta poslední v kolekci.
        /// </summary>
        public void NextCard()
        {
            if (_currentCardIndex >= _cards.Count - 1)   // není další karta v kolekci
            {
                throw new IndexOutOfCardsRangeException("Toto je poslední karta!");
            }
            else
            {
                _currentCardIndex++;
            }
            CallChange("CurrentCardAnswer");
            CallChange("CurrentCardQuestion");
        }

        /// <summary>
        /// Změní právě prohlíženou kartu na předchozí v kolekci. Vyvolá výjimku, pokud je karta první v kolekci.
        /// </summary>
        public void PreviousCard()
        {
            if (_currentCardIndex <= 0)
            {
                throw new IndexOutOfCardsRangeException("Toto je první karta!");
            }
            else
            {
                _currentCardIndex--;
            }
            CallChange("CurrentCardAnswer");
            CallChange("CurrentCardQuestion");
        }

        /// <summary>
        /// Nahraje kolekci karet ze souboru kvízu.
        /// </summary>
        public void Import()
        {
            _cards.Clear();

            using (FileStream fs = new FileStream(_path, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                _cards = (List<Card>)bin.Deserialize(fs);
            }

        }

        /// <summary>
        /// Uloží kolekci karet do souboru kvízu. Vyvolá výjimku, pokud má některá z karet v kolekci prázdou otázku/odpověď.
        /// </summary>
        public void Save()
        {
            if (!ValidateCards()) // kontrola, zda mají všechny karty vyplněnou otázku i odpověď
            {
                throw new InvalidCardsException("Existují nevyplněné karty");
            }

            using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(fs, _cards);
            }
        }

        /// <summary>
        /// Validace, zda mají všechny karty v kolekci vyplněnou otázku i odpověď.
        /// </summary>
        /// <returns>True, pokud validace proběhla v přádku. False, pokdu kolekce není validní.</returns>
        private bool ValidateCards()
        {
            foreach (Card c in _cards)
            {
                if ((c.Question.Trim() == "") || (c.Answer.Trim() == ""))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Přidá novou prázdnou kartu do kolekce.
        /// </summary>
        public void AddNewCard()
        {
            _cards.Add(new Card("", ""));
        }

        /// <summary>
        /// Vymaže právě prohlíženou kartu z kolekce. Pokud je v kolekci jedna karta, tak vymaže obsaj její otázky a odpovědi.
        /// </summary>
        public void DeleteCurrentCard()
        {            
            if (_cards.Count == 1)   // v kolekci je pouze jedna karta
            {
                CurrentCardAnswer = "";
                CurrentCardQuestion = "";                
            }
            else
            {
                _cards.RemoveAt(_currentCardIndex);
                if (_currentCardIndex >= _cards.Count) // pokud byla karta poslední, snížit index
                {
                    _currentCardIndex = _cards.Count - 1;
                }
            }
            CallChange("CurrentCardAnswer");
            CallChange("CurrentCardQuestion");
        }

        /// <summary>
        /// Vypíše název kvízu.
        /// </summary>
        /// <returns>Vrací název kvízu.</returns>
        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(_path);
        }

        /// <summary>
        /// Vyvolá událost pro změnu vlastnosti.
        /// </summary>
        /// <param name="vlastnost">Název vlastnoti.</param>
        private void CallChange(string vlastnost)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
            }
        }
    }
}

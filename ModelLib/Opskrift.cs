using System;
using System.Collections.Generic;

namespace ModelLib
{
    public class Opskrift
    {
        private int _id;
        private string _navn;
        private string _fremgangsmaade;
        private int _tid;
        private string _billede;
        private List<string> _ingredienser;

        public Opskrift(int id, string navn, string fremgangsmaade, int tid, string billede, List<string> ingredienser)
        {
            _id = id;
            _navn = navn;
            _fremgangsmaade = fremgangsmaade;
            _tid = tid;
            _ingredienser = ingredienser;
            _billede = billede;
        }

        public Opskrift()
        {
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Navn
        {
            get => _navn;
            set => _navn = value;
        }

        public string Fremgangsmaade
        {
            get => _fremgangsmaade;
            set => _fremgangsmaade = value;
        }

        public int Tid
        {
            get => _tid;
            set => _tid = value;
        }

        public string Billede
        {
            get => _billede;
            set => _billede = value;
        }

        public List<string> Ingredienser
        {
            get => _ingredienser;
            set => _ingredienser = value;
        }

        
        public override string ToString()
        {
            return $"{_id},{_navn},{_fremgangsmaade},{_tid}, {_billede}, {_ingredienser}";
        }
    }
}

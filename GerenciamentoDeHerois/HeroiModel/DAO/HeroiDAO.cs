using HeroiModel.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeroiModel.DAO
{
    public class HeroiDAO
    {
        private string databaseName;
        private static HeroiDAO instance = null;
        private Hero Hero { get; set; }

        private HeroiDAO()
        {
            Hero = new Hero();
            databaseName = "DATABASE";
            ReadFile();
        }

        public static HeroiDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HeroiDAO();
                }
                return instance;
            }
        }

        public void Add(Heroi heroi)
        {
            var DataCadastro = DateTime.Now;
            heroi.NomeCompleto = heroi.NomeCompleto.Trim();
            heroi.Codinome = heroi.Codinome.Trim();
            heroi.DataCadastro = DataCadastro;
            heroi.Nascimento = heroi.Nascimento;

            heroi.Poder = heroi.Poder;
            if (FindById(heroi.Id) == null)
            {
                if (heroi.Id < 1)
                {
                    heroi.Id = Hero.Index;
                    Hero.Index++;
                }
                Hero.List.Add(heroi);
            }
            else
            {
                throw new Exception("Id da coluna já existe!");
            }
            SaveFile();
        }

        public List<Heroi> GetAll()
        {
            return new List<Heroi>(Hero.List);
        }

        public Heroi FindById(int id)
        {
            foreach (var heroi in Hero.List)
            {
                if (heroi.Id == id)
                {
                    return heroi;
                }
            }
            return null;
        }

        public List<Heroi> FindByNameAndSurname(string codinome)
        {
            List<Heroi> result = new List<Heroi>();
            codinome = codinome.Trim();
            if (codinome != "")
            {
                foreach (var heroi in Hero.List)
                {
                    if ((heroi.NomeCompleto.ToLower() + " " + heroi.Codinome.ToLower()).Contains(codinome.ToLower()))
                    {
                        result.Add(heroi);
                    }
                }
            }
            return result;
        }

        public void Update(Heroi heroi)
        {
            Heroi original = FindById(heroi.Id);
            if (original != null)
            {
                Hero.List.Insert(Hero.List.IndexOf(original), heroi);
                Hero.List.Remove(original);
            }
            else
            {
                throw new Exception("Heroi não encontrado");
            }
            SaveFile();
        }

        public void Remove(int id)
        {
            Hero.List.Remove(FindById(id));
            SaveFile();
        }

        private void ReadFile()
        {
            string json;
            try
            {
                using (StreamReader streamReader = new StreamReader(databaseName))
                {
                    json = streamReader.ReadLine();
                }
                Hero = JsonConvert.DeserializeObject<Hero>(json);
            }
            catch (FileNotFoundException)
            {
                StreamWriter streamWriter = new StreamWriter(databaseName);
                streamWriter.Close();
                Hero.List = new List<Heroi>();
                Hero.Index = 1;
            }
        }

        private void SaveFile()
        {
            string json = JsonConvert.SerializeObject(Hero);
            using (StreamWriter streamWriter = new StreamWriter(databaseName))
            {
                streamWriter.Write(json);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Donante
    {
        //Atributos
        private string nombre;
        private int dni;
        private int edad;
        private string grupoSanguineo;

        //Contructor
        public Donante()
        {
                    
        }
        //Getters y Setters
        public string Nombre {get; set;}
        public int Dni {get; set;}  
        public int Edad {get; set;} 
        public string GrupoSanguineo {get; set;}    


    }
}

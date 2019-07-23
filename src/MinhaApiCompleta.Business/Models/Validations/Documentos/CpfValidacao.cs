using System;
using System.Collections.Generic;
using System.Text;

namespace MinhaApiCompleta.Business.Models.Validations.Documentos
{
    public class CpfValidacao
    {
        public const int TamanhoCpf = 11;

        public static bool Validar(string cpf)
        {
            var cpfNumeros = Utils.ApenasNumeros(cpf);

            if (!TamanhoValido(cpfNumeros)) return false;

            return !TemDigitosRepetidos(cpfNumeros) && TemDigitosValidos(cpfNumeros);
        }

        private static bool TamanhoValido(string valor) =>
            valor.Length == TamanhoCpf;

        private static bool TemDigitosRepetidos(string valor) => true;
    }
}
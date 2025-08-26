using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class EmprestimoPessoalModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public EmprestimoPessoalModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public decimal ValorEmprestimo { get; set; }

        [BindProperty]
        public decimal TaxaJuros { get; set; } = 6.5m;

        [BindProperty]
        public int PrazoMeses { get; set; } = 24;

        [BindProperty]
        public string TipoEmprestimo { get; set; } = "Pessoal";

        // Propriedades calculadas
        public decimal PrestacaoMensal { get; set; }
        public decimal TotalJuros { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal CET { get; set; }
        public decimal ValorIOF { get; set; }
        public bool CalculoRealizado { get; set; }

        public void OnGet()
        {
            CalculoRealizado = false;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var modelo = new Models.EmprestimoPessoalModel
                {
                    ValorEmprestimo = ValorEmprestimo,
                    TaxaJuros = TaxaJuros,
                    PrazoMeses = PrazoMeses,
                    TipoEmprestimo = TipoEmprestimo
                };

                var resultado = _calculadoraService.CalcularEmprestimoPessoal(modelo);

                PrestacaoMensal = resultado.PrestacaoMensal;
                TotalJuros = resultado.TotalJuros;
                ValorTotal = resultado.ValorTotal;
                CET = resultado.CET;
                ValorIOF = resultado.ValorIOF;
                CalculoRealizado = resultado.CalculoRealizado;

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao calcular o empréstimo. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class JurosCompostosModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public JurosCompostosModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public decimal ValorInicial { get; set; }

        [BindProperty]
        public decimal AporteMensal { get; set; }

        [BindProperty]
        public decimal TaxaJurosMensal { get; set; } = 0.8m;

        [BindProperty]
        public int PeriodoMeses { get; set; } = 60;

        // Propriedades calculadas
        public decimal ValorFinal { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal TotalJuros { get; set; }
        public decimal Rentabilidade { get; set; }
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
                var modelo = new Models.JurosCompostosModel
                {
                    ValorInicial = ValorInicial,
                    AporteMensal = AporteMensal,
                    TaxaJurosMensal = TaxaJurosMensal,
                    PeriodoMeses = PeriodoMeses
                };

                var resultado = _calculadoraService.CalcularJurosCompostos(modelo);

                ValorFinal = resultado.ValorFinal;
                TotalInvestido = resultado.TotalInvestido;
                TotalJuros = resultado.TotalJuros;
                Rentabilidade = resultado.Rentabilidade;
                CalculoRealizado = resultado.CalculoRealizado;

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao calcular os juros compostos. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class FinanciamentoImobiliarioModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public FinanciamentoImobiliarioModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public decimal ValorImovel { get; set; }

        [BindProperty]
        public decimal ValorEntrada { get; set; }

        [BindProperty]
        public decimal TaxaJuros { get; set; } = 10.5m;

        [BindProperty]
        public int PrazoAnos { get; set; } = 30;

        // Propriedades calculadas
        public decimal ValorFinanciado { get; set; }
        public decimal PrestacaoSAC { get; set; }
        public decimal PrestacaoPRICE { get; set; }
        public decimal TotalJurosSAC { get; set; }
        public decimal TotalJurosPRICE { get; set; }
        public decimal ValorTotalSAC { get; set; }
        public decimal ValorTotalPRICE { get; set; }
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
                var modelo = new Models.FinanciamentoImobiliarioModel
                {
                    ValorImovel = ValorImovel,
                    ValorEntrada = ValorEntrada,
                    TaxaJuros = TaxaJuros,
                    PrazoAnos = PrazoAnos
                };

                var resultado = _calculadoraService.CalcularFinanciamentoImobiliario(modelo);

                ValorFinanciado = resultado.ValorFinanciado;
                PrestacaoSAC = resultado.PrestacaoSAC;
                PrestacaoPRICE = resultado.PrestacaoPRICE;
                TotalJurosSAC = resultado.TotalJurosSAC;
                TotalJurosPRICE = resultado.TotalJurosPRICE;
                ValorTotalSAC = resultado.ValorTotalSAC;
                ValorTotalPRICE = resultado.ValorTotalPRICE;
                CalculoRealizado = resultado.CalculoRealizado;

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao calcular o financiamento. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
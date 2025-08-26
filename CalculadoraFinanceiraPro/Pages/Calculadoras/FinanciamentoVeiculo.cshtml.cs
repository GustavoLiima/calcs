using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class FinanciamentoVeiculoModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public FinanciamentoVeiculoModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public decimal ValorVeiculo { get; set; }

        [BindProperty]
        public decimal ValorEntrada { get; set; }

        [BindProperty]
        public decimal TaxaJuros { get; set; } = 1.5m;

        [BindProperty]
        public int PrazoParcelas { get; set; } = 60;

        [BindProperty]
        public bool VeiculoNovo { get; set; } = true;

        // Propriedades calculadas
        public decimal ValorFinanciado { get; set; }
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
                var modelo = new Models.FinanciamentoVeiculoModel
                {
                    ValorVeiculo = ValorVeiculo,
                    ValorEntrada = ValorEntrada,
                    TaxaJuros = TaxaJuros,
                    PrazoParcelas = PrazoParcelas,
                    VeiculoNovo = VeiculoNovo
                };

                var resultado = _calculadoraService.CalcularFinanciamentoVeiculo(modelo);

                ValorFinanciado = resultado.ValorFinanciado;
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
                ModelState.AddModelError(string.Empty, "Erro ao calcular o financiamento. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
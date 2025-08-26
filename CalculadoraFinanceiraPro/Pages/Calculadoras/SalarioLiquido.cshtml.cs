using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class SalarioLiquidoModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public SalarioLiquidoModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public SalarioLiquidoModel SalarioLiquidoData { get; set; } = new();

        // Propriedades para binding individual
        [BindProperty]
        public decimal SalarioBruto { get; set; }

        [BindProperty]
        public int Dependentes { get; set; }

        // Propriedades calculadas
        public decimal DescontoINSS { get; set; }
        public decimal DescontoIRRF { get; set; }
        public decimal SalarioLiquido { get; set; }
        public decimal AliquotaINSS { get; set; }
        public decimal AliquotaIRRF { get; set; }
        public bool CalculoRealizado { get; set; }

        public void OnGet()
        {
            // Valores padrão para demonstração
            SalarioBruto = 0;
            Dependentes = 0;
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
                // Criar modelo para cálculo
                var modelo = new Models.SalarioLiquidoModel
                {
                    SalarioBruto = SalarioBruto,
                    Dependentes = Dependentes
                };

                // Calcular
                var resultado = _calculadoraService.CalcularSalarioLiquido(modelo);

                // Atribuir resultados
                DescontoINSS = resultado.DescontoINSS;
                DescontoIRRF = resultado.DescontoIRRF;
                SalarioLiquido = resultado.SalarioLiquido;
                AliquotaINSS = resultado.AliquotaINSS;
                AliquotaIRRF = resultado.AliquotaIRRF;
                CalculoRealizado = resultado.CalculoRealizado;

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao calcular o salário líquido. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
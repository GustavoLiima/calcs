using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class DecimoTerceiroModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public DecimoTerceiroModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public decimal SalarioBruto { get; set; }

        [BindProperty]
        public int MesesTrabalhados { get; set; } = 12;

        [BindProperty]
        public int Dependentes { get; set; } = 0;

        [BindProperty]
        public decimal MediaHorasExtras { get; set; } = 0;

        [BindProperty]
        public decimal MediaComissoes { get; set; } = 0;

        [BindProperty]
        public decimal Adiantamento { get; set; } = 0;

        // Propriedades calculadas
        public decimal DecimoTerceiroBruto { get; set; }
        public decimal DecimoTerceiroLiquido { get; set; }
        public decimal DescontoINSS { get; set; }
        public decimal DescontoIRRF { get; set; }
        public decimal AliquotaINSS { get; set; }
        public decimal AliquotaIRRF { get; set; }
        public decimal ValorAReceber { get; set; }
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
                var modelo = new Models.DecimoTerceiroModel
                {
                    SalarioBruto = SalarioBruto,
                    MesesTrabalhados = MesesTrabalhados,
                    Dependentes = Dependentes,
                    MediaHorasExtras = MediaHorasExtras,
                    MediaComissoes = MediaComissoes,
                    Adiantamento = Adiantamento
                };

                var resultado = _calculadoraService.CalcularDecimoTerceiro(modelo);

                DecimoTerceiroBruto = resultado.DecimoTerceiroBruto;
                DecimoTerceiroLiquido = resultado.DecimoTerceiroLiquido;
                DescontoINSS = resultado.DescontoINSS;
                DescontoIRRF = resultado.DescontoIRRF;
                AliquotaINSS = resultado.AliquotaINSS;
                AliquotaIRRF = resultado.AliquotaIRRF;
                ValorAReceber = resultado.ValorAReceber;
                CalculoRealizado = resultado.CalculoRealizado;

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao calcular o 13º salário. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
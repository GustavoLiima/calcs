using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraFinanceiraPro.Models;
using CalculadoraFinanceiraPro.Services;

namespace CalculadoraFinanceiraPro.Pages.Calculadoras
{
    public class HoraExtraModel : PageModel
    {
        private readonly CalculadoraService _calculadoraService;

        public HoraExtraModel(CalculadoraService calculadoraService)
        {
            _calculadoraService = calculadoraService;
        }

        [BindProperty]
        public decimal SalarioBase { get; set; }

        [BindProperty]
        public int JornadaSemanal { get; set; } = 40;

        [BindProperty]
        public decimal HorasExtras50 { get; set; }

        [BindProperty]
        public decimal HorasExtras100 { get; set; }

        [BindProperty]
        public decimal HorasNoturnas { get; set; }

        [BindProperty]
        public bool CalcularDSR { get; set; } = true;

        // Propriedades calculadas
        public decimal ValorHoraNormal { get; set; }
        public decimal ValorHorasExtras50 { get; set; }
        public decimal ValorHorasExtras100 { get; set; }
        public decimal ValorAdicionalNoturno { get; set; }
        public decimal ValorDSR { get; set; }
        public decimal TotalAdicional { get; set; }
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
                var modelo = new Models.HoraExtraModel
                {
                    SalarioBase = SalarioBase,
                    JornadaSemanal = JornadaSemanal,
                    HorasExtras50 = HorasExtras50,
                    HorasExtras100 = HorasExtras100,
                    HorasNoturnas = HorasNoturnas,
                    CalcularDSR = CalcularDSR
                };

                var resultado = _calculadoraService.CalcularHoraExtra(modelo);

                ValorHoraNormal = resultado.ValorHoraNormal;
                ValorHorasExtras50 = resultado.ValorHorasExtras50;
                ValorHorasExtras100 = resultado.ValorHorasExtras100;
                ValorAdicionalNoturno = resultado.ValorAdicionalNoturno;
                ValorDSR = resultado.ValorDSR;
                TotalAdicional = resultado.TotalAdicional;
                CalculoRealizado = resultado.CalculoRealizado;

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao calcular as horas extras. Verifique os valores informados.");
                return Page();
            }
        }
    }
}
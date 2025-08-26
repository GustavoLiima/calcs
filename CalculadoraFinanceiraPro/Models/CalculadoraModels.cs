using System.ComponentModel.DataAnnotations;

namespace CalculadoraFinanceiraPro.Models
{
    /// <summary>
    /// Modelo base para todas as calculadoras
    /// </summary>
    public abstract class BaseCalculadoraModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool CalculoRealizado { get; set; } = false;
    }

    /// <summary>
    /// Modelo para calculadora de salário líquido
    /// </summary>
    public class SalarioLiquidoModel : BaseCalculadoraModel
    {
        [Display(Name = "Salário Bruto (R$)")]
        [Required(ErrorMessage = "O salário bruto é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O salário deve ser maior que zero")]
        public decimal SalarioBruto { get; set; }

        [Display(Name = "Número de Dependentes")]
        [Range(0, 20, ErrorMessage = "O número de dependentes deve estar entre 0 e 20")]
        public int Dependentes { get; set; } = 0;

        [Display(Name = "Desconto INSS")]
        public decimal DescontoINSS { get; set; }

        [Display(Name = "Desconto IRRF")]
        public decimal DescontoIRRF { get; set; }

        [Display(Name = "Salário Líquido")]
        public decimal SalarioLiquido { get; set; }

        [Display(Name = "Alíquota INSS (%)")]
        public decimal AliquotaINSS { get; set; }

        [Display(Name = "Alíquota IRRF (%)")]
        public decimal AliquotaIRRF { get; set; }
    }

    /// <summary>
    /// Modelo para calculadora de financiamento imobiliário
    /// </summary>
    public class FinanciamentoImobiliarioModel : BaseCalculadoraModel
    {
        [Display(Name = "Valor do Imóvel (R$)")]
        [Required(ErrorMessage = "O valor do imóvel é obrigatório")]
        [Range(1000, 10000000, ErrorMessage = "Valor deve estar entre R$ 1.000 e R$ 10.000.000")]
        public decimal ValorImovel { get; set; }

        [Display(Name = "Valor de Entrada (R$)")]
        [Range(0, double.MaxValue, ErrorMessage = "A entrada deve ser maior ou igual a zero")]
        public decimal ValorEntrada { get; set; }

        [Display(Name = "Taxa de Juros Anual (%)")]
        [Required(ErrorMessage = "A taxa de juros é obrigatória")]
        [Range(0.1, 50, ErrorMessage = "Taxa deve estar entre 0,1% e 50%")]
        public decimal TaxaJuros { get; set; } = 10.5m;

        [Display(Name = "Prazo em Anos")]
        [Required(ErrorMessage = "O prazo é obrigatório")]
        [Range(1, 35, ErrorMessage = "Prazo deve estar entre 1 e 35 anos")]
        public int PrazoAnos { get; set; } = 30;

        [Display(Name = "Sistema de Amortização")]
        public string SistemaAmortizacao { get; set; } = "SAC";

        // Resultados
        [Display(Name = "Valor a Financiar")]
        public decimal ValorFinanciado { get; set; }

        [Display(Name = "Prestação Mensal (SAC - Primeira)")]
        public decimal PrestacaoSAC { get; set; }

        [Display(Name = "Prestação Mensal (PRICE)")]
        public decimal PrestacaoPRICE { get; set; }

        [Display(Name = "Total de Juros (SAC)")]
        public decimal TotalJurosSAC { get; set; }

        [Display(Name = "Total de Juros (PRICE)")]
        public decimal TotalJurosPRICE { get; set; }

        [Display(Name = "Valor Total Pago (SAC)")]
        public decimal ValorTotalSAC { get; set; }

        [Display(Name = "Valor Total Pago (PRICE)")]
        public decimal ValorTotalPRICE { get; set; }
    }

    /// <summary>
    /// Modelo para calculadora de juros compostos
    /// </summary>
    public class JurosCompostosModel : BaseCalculadoraModel
    {
        [Display(Name = "Valor Inicial (R$)")]
        [Required(ErrorMessage = "O valor inicial é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor inicial deve ser maior que zero")]
        public decimal ValorInicial { get; set; }

        [Display(Name = "Aporte Mensal (R$)")]
        [Range(0, double.MaxValue, ErrorMessage = "O aporte mensal deve ser maior ou igual a zero")]
        public decimal AporteMensal { get; set; } = 0;

        [Display(Name = "Taxa de Juros Mensal (%)")]
        [Required(ErrorMessage = "A taxa de juros é obrigatória")]
        [Range(0.01, 50, ErrorMessage = "Taxa deve estar entre 0,01% e 50%")]
        public decimal TaxaJurosMensal { get; set; }

        [Display(Name = "Período (meses)")]
        [Required(ErrorMessage = "O período é obrigatório")]
        [Range(1, 1200, ErrorMessage = "Período deve estar entre 1 e 1.200 meses")]
        public int PeriodoMeses { get; set; }

        // Resultados
        [Display(Name = "Valor Final")]
        public decimal ValorFinal { get; set; }

        [Display(Name = "Total Investido")]
        public decimal TotalInvestido { get; set; }

        [Display(Name = "Total de Juros")]
        public decimal TotalJuros { get; set; }

        [Display(Name = "Rentabilidade (%)")]
        public decimal Rentabilidade { get; set; }
    }

    /// <summary>
    /// Modelo para calculadora de financiamento de veículos
    /// </summary>
    public class FinanciamentoVeiculoModel : BaseCalculadoraModel
    {
        [Display(Name = "Valor do Veículo (R$)")]
        [Required(ErrorMessage = "O valor do veículo é obrigatório")]
        [Range(1000, 1000000, ErrorMessage = "Valor deve estar entre R$ 1.000 e R$ 1.000.000")]
        public decimal ValorVeiculo { get; set; }

        [Display(Name = "Valor de Entrada (R$)")]
        [Range(0, double.MaxValue, ErrorMessage = "A entrada deve ser maior ou igual a zero")]
        public decimal ValorEntrada { get; set; }

        [Display(Name = "Taxa de Juros Anual (%)")]
        [Required(ErrorMessage = "A taxa de juros é obrigatória")]
        [Range(0.1, 50, ErrorMessage = "Taxa deve estar entre 0,1% e 50%")]
        public decimal TaxaJuros { get; set; } = 15.5m;

        [Display(Name = "Prazo em Meses")]
        [Required(ErrorMessage = "O prazo é obrigatório")]
        [Range(6, 72, ErrorMessage = "Prazo deve estar entre 6 e 72 meses")]
        public int PrazoMeses { get; set; } = 48;

        // Resultados
        [Display(Name = "Valor a Financiar")]
        public decimal ValorFinanciado { get; set; }

        [Display(Name = "Prestação Mensal")]
        public decimal PrestacaoMensal { get; set; }

        [Display(Name = "Total de Juros")]
        public decimal TotalJuros { get; set; }

        [Display(Name = "Valor Total Pago")]
        public decimal ValorTotalPago { get; set; }

        [Display(Name = "CET (Custo Efetivo Total) %")]
        public decimal CET { get; set; }
    }

    /// <summary>
    /// Modelo para calculadora de empréstimo pessoal
    /// </summary>
    public class EmprestimoPessoalModel : BaseCalculadoraModel
    {
        [Display(Name = "Valor do Empréstimo (R$)")]
        [Required(ErrorMessage = "O valor do empréstimo é obrigatório")]
        [Range(100, 500000, ErrorMessage = "Valor deve estar entre R$ 100 e R$ 500.000")]
        public decimal ValorEmprestimo { get; set; }

        [Display(Name = "Taxa de Juros Mensal (%)")]
        [Required(ErrorMessage = "A taxa de juros é obrigatória")]
        [Range(0.5, 20, ErrorMessage = "Taxa deve estar entre 0,5% e 20% ao mês")]
        public decimal TaxaJuros { get; set; } = 6.5m;

        [Display(Name = "Prazo em Meses")]
        [Required(ErrorMessage = "O prazo é obrigatório")]
        [Range(3, 120, ErrorMessage = "Prazo deve estar entre 3 e 120 meses")]
        public int PrazoMeses { get; set; } = 24;

        [Display(Name = "Tipo de Empréstimo")]
        public string TipoEmprestimo { get; set; } = "Pessoal";

        // Resultados
        [Display(Name = "Prestação Mensal")]
        public decimal PrestacaoMensal { get; set; }

        [Display(Name = "Total de Juros")]
        public decimal TotalJuros { get; set; }

        [Display(Name = "Valor Total a Pagar")]
        public decimal ValorTotal { get; set; }

        [Display(Name = "CET (Custo Efetivo Total) %")]
        public decimal CET { get; set; }

        [Display(Name = "Valor do IOF")]
        public decimal ValorIOF { get; set; }
    }

    /// <summary>
    /// Modelo para calculadora de hora extra
    /// </summary>
    public class HoraExtraModel : BaseCalculadoraModel
    {
        [Display(Name = "Salário Base (R$)")]
        [Required(ErrorMessage = "O salário base é obrigatório")]
        [Range(1, double.MaxValue, ErrorMessage = "O salário deve ser maior que zero")]
        public decimal SalarioBase { get; set; }

        [Display(Name = "Jornada Semanal (horas)")]
        [Required(ErrorMessage = "A jornada semanal é obrigatória")]
        [Range(20, 60, ErrorMessage = "Jornada deve estar entre 20 e 60 horas")]
        public int JornadaSemanal { get; set; } = 40;

        [Display(Name = "Horas Extras 50%")]
        [Range(0, 300, ErrorMessage = "Horas extras devem estar entre 0 e 300")]
        public decimal HorasExtras50 { get; set; } = 0;

        [Display(Name = "Horas Extras 100%")]
        [Range(0, 300, ErrorMessage = "Horas extras devem estar entre 0 e 300")]
        public decimal HorasExtras100 { get; set; } = 0;

        [Display(Name = "Horas Noturnas")]
        [Range(0, 500, ErrorMessage = "Horas noturnas devem estar entre 0 e 500")]
        public decimal HorasNoturnas { get; set; } = 0;

        [Display(Name = "Calcular DSR")]
        public bool CalcularDSR { get; set; } = true;

        // Resultados
        [Display(Name = "Valor da Hora Normal")]
        public decimal ValorHoraNormal { get; set; }

        [Display(Name = "Valor das Horas Extras 50%")]
        public decimal ValorHorasExtras50 { get; set; }

        [Display(Name = "Valor das Horas Extras 100%")]
        public decimal ValorHorasExtras100 { get; set; }

        [Display(Name = "Valor do Adicional Noturno")]
        public decimal ValorAdicionalNoturno { get; set; }

        [Display(Name = "Valor do DSR")]
        public decimal ValorDSR { get; set; }

        [Display(Name = "Total de Adicional")]
        public decimal TotalAdicional { get; set; }
    }

    /// <summary>
    /// Modelo para calculadora de 13º salário
    /// </summary>
    public class DecimoTerceiroModel : BaseCalculadoraModel
    {
        [Display(Name = "Salário Bruto (R$)")]
        [Required(ErrorMessage = "O salário bruto é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O salário deve ser maior que zero")]
        public decimal SalarioBruto { get; set; }

        [Display(Name = "Meses Trabalhados")]
        [Required(ErrorMessage = "O número de meses é obrigatório")]
        [Range(1, 12, ErrorMessage = "Meses deve estar entre 1 e 12")]
        public int MesesTrabalhados { get; set; } = 12;

        [Display(Name = "Número de Dependentes")]
        [Range(0, 20, ErrorMessage = "O número de dependentes deve estar entre 0 e 20")]
        public int Dependentes { get; set; } = 0;

        [Display(Name = "Média de Horas Extras")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor deve ser positivo")]
        public decimal MediaHorasExtras { get; set; } = 0;

        [Display(Name = "Média de Comissões")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor deve ser positivo")]
        public decimal MediaComissoes { get; set; } = 0;

        [Display(Name = "Adiantamento")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor deve ser positivo")]
        public decimal Adiantamento { get; set; } = 0;

        // Resultados
        [Display(Name = "13º Salário Bruto")]
        public decimal DecimoTerceiroBruto { get; set; }

        [Display(Name = "13º Salário Líquido")]
        public decimal DecimoTerceiroLiquido { get; set; }

        [Display(Name = "Desconto INSS")]
        public decimal DescontoINSS { get; set; }

        [Display(Name = "Desconto IRRF")]
        public decimal DescontoIRRF { get; set; }

        [Display(Name = "Alíquota INSS (%)")]
        public decimal AliquotaINSS { get; set; }

        [Display(Name = "Alíquota IRRF (%)")]
        public decimal AliquotaIRRF { get; set; }

        [Display(Name = "Valor a Receber")]
        public decimal ValorAReceber { get; set; }

        [Display(Name = "Taxa de Juros Anual (%)")]
        public decimal TaxaJurosAnual { get; set; }
    }

    /// <summary>
    /// Modelo para resultado genérico de calculadoras
    /// </summary>
    public class ResultadoCalculadora
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public Dictionary<string, object> Dados { get; set; } = new Dictionary<string, object>();
    }
}
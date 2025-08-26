using CalculadoraFinanceiraPro.Models;

namespace CalculadoraFinanceiraPro.Services
{
    /// <summary>
    /// Serviço para cálculos financeiros
    /// </summary>
    public class CalculadoraService
    {
        /// <summary>
        /// Calcula o salário líquido considerando INSS e IRRF
        /// </summary>
        public SalarioLiquidoModel CalcularSalarioLiquido(SalarioLiquidoModel model)
        {
            try
            {
                // Cálculo INSS 2024
                decimal inss = CalcularINSS(model.SalarioBruto);
                model.DescontoINSS = inss;
                
                // Base de cálculo para IRRF = Salário - INSS - (Dependentes * 189.59)
                decimal baseIRRF = model.SalarioBruto - inss - (model.Dependentes * 189.59m);
                
                // Cálculo IRRF 2024
                decimal irrf = CalcularIRRF(baseIRRF);
                model.DescontoIRRF = irrf;
                
                // Salário líquido
                model.SalarioLiquido = model.SalarioBruto - inss - irrf;
                
                // Alíquotas efetivas
                model.AliquotaINSS = model.SalarioBruto > 0 ? (inss / model.SalarioBruto) * 100 : 0;
                model.AliquotaIRRF = model.SalarioBruto > 0 ? (irrf / model.SalarioBruto) * 100 : 0;
                
                model.CalculoRealizado = true;
                return model;
            }
            catch (Exception)
            {
                model.CalculoRealizado = false;
                return model;
            }
        }

        /// <summary>
        /// Calcula financiamento imobiliário (SAC e PRICE)
        /// </summary>
        public FinanciamentoImobiliarioModel CalcularFinanciamentoImobiliario(FinanciamentoImobiliarioModel model)
        {
            try
            {
                model.ValorFinanciado = model.ValorImovel - model.ValorEntrada;
                
                if (model.ValorFinanciado <= 0)
                {
                    model.CalculoRealizado = false;
                    return model;
                }

                decimal taxaMensal = model.TaxaJuros / 100 / 12;
                int totalParcelas = model.PrazoAnos * 12;

                // Cálculo PRICE
                decimal fatorPrice = (decimal)Math.Pow((double)(1 + taxaMensal), totalParcelas);
                model.PrestacaoPRICE = model.ValorFinanciado * (taxaMensal * fatorPrice) / (fatorPrice - 1);
                model.ValorTotalPRICE = model.PrestacaoPRICE * totalParcelas;
                model.TotalJurosPRICE = model.ValorTotalPRICE - model.ValorFinanciado;

                // Cálculo SAC
                decimal amortizacao = model.ValorFinanciado / totalParcelas;
                decimal jurosPrimeiraParcela = model.ValorFinanciado * taxaMensal;
                model.PrestacaoSAC = amortizacao + jurosPrimeiraParcela;
                
                // Cálculo total SAC (soma de PA)
                decimal somaJuros = 0;
                decimal saldoDevedor = model.ValorFinanciado;
                
                for (int i = 0; i < totalParcelas; i++)
                {
                    decimal juros = saldoDevedor * taxaMensal;
                    somaJuros += juros;
                    saldoDevedor -= amortizacao;
                }
                
                model.TotalJurosSAC = somaJuros;
                model.ValorTotalSAC = model.ValorFinanciado + model.TotalJurosSAC;

                model.CalculoRealizado = true;
                return model;
            }
            catch (Exception)
            {
                model.CalculoRealizado = false;
                return model;
            }
        }

        /// <summary>
        /// Calcula juros compostos com aportes
        /// </summary>
        public JurosCompostosModel CalcularJurosCompostos(JurosCompostosModel model)
        {
            try
            {
                decimal taxaDecimal = model.TaxaJurosMensal / 100;
                decimal valorAtual = model.ValorInicial;
                
                model.TotalInvestido = model.ValorInicial + (model.AporteMensal * model.PeriodoMeses);

                // Cálculo com aportes mensais
                for (int mes = 1; mes <= model.PeriodoMeses; mes++)
                {
                    // Aplica juros sobre o valor atual
                    valorAtual *= (1 + taxaDecimal);
                    
                    // Adiciona aporte mensal
                    valorAtual += model.AporteMensal;
                }

                model.ValorFinal = valorAtual;
                model.TotalJuros = model.ValorFinal - model.TotalInvestido;
                model.Rentabilidade = model.TotalInvestido > 0 ? 
                    ((model.ValorFinal - model.TotalInvestido) / model.TotalInvestido) * 100 : 0;

                model.CalculoRealizado = true;
                return model;
            }
            catch (Exception)
            {
                model.CalculoRealizado = false;
                return model;
            }
        }

        /// <summary>
        /// Calcula financiamento de veículos
        /// </summary>
        public FinanciamentoVeiculoModel CalcularFinanciamentoVeiculo(FinanciamentoVeiculoModel model)
        {
            try
            {
                model.ValorFinanciado = model.ValorVeiculo - model.ValorEntrada;
                
                if (model.ValorFinanciado <= 0)
                {
                    model.CalculoRealizado = false;
                    return model;
                }

                decimal taxaMensal = model.TaxaJuros / 100 / 12;
                
                // Cálculo da prestação (PRICE)
                decimal fator = (decimal)Math.Pow((double)(1 + taxaMensal), model.PrazoMeses);
                model.PrestacaoMensal = model.ValorFinanciado * (taxaMensal * fator) / (fator - 1);
                
                model.ValorTotalPago = model.PrestacaoMensal * model.PrazoMeses;
                model.TotalJuros = model.ValorTotalPago - model.ValorFinanciado;
                
                // CET aproximado (considera apenas juros, sem tarifas)
                model.CET = model.TaxaJuros;

                model.CalculoRealizado = true;
                return model;
            }
            catch (Exception)
            {
                model.CalculoRealizado = false;
                return model;
            }
        }

        /// <summary>
        /// Calcula empréstimo pessoal
        /// </summary>
        public EmprestimoPessoalModel CalcularEmprestimoPessoal(EmprestimoPessoalModel model)
        {
            try
            {
                decimal taxaDecimal = model.TaxaJurosMensal / 100;
                
                // Cálculo da prestação (PRICE)
                decimal fator = (decimal)Math.Pow((double)(1 + taxaDecimal), model.PrazoMeses);
                model.PrestacaoMensal = model.ValorEmprestimo * (taxaDecimal * fator) / (fator - 1);
                
                model.ValorTotalPago = model.PrestacaoMensal * model.PrazoMeses;
                model.TotalJuros = model.ValorTotalPago - model.ValorEmprestimo;
                
                // Taxa anual equivalente
                model.TaxaJurosAnual = ((decimal)Math.Pow((double)(1 + taxaDecimal), 12) - 1) * 100;

                model.CalculoRealizado = true;
                return model;
            }
            catch (Exception)
            {
                model.CalculoRealizado = false;
                return model;
            }
        }

        #region Métodos Auxiliares

        /// <summary>
        /// Calcula INSS conforme tabela 2024
        /// </summary>
        private decimal CalcularINSS(decimal salario)
        {
            decimal inss = 0;

            // Faixas INSS 2024
            if (salario <= 1412.00m)
                inss = salario * 0.075m;
            else if (salario <= 2666.68m)
                inss = 105.90m + (salario - 1412.00m) * 0.09m;
            else if (salario <= 4000.03m)
                inss = 218.82m + (salario - 2666.68m) * 0.12m;
            else if (salario <= 7786.02m)
                inss = 378.82m + (salario - 4000.03m) * 0.14m;
            else
                inss = 908.85m; // Teto INSS

            return Math.Round(inss, 2);
        }

        /// <summary>
        /// Calcula IRRF conforme tabela 2024
        /// </summary>
        private decimal CalcularIRRF(decimal baseCalculo)
        {
            if (baseCalculo <= 2112.00m)
                return 0;
            else if (baseCalculo <= 2826.65m)
                return baseCalculo * 0.075m - 158.40m;
            else if (baseCalculo <= 3751.05m)
                return baseCalculo * 0.15m - 370.40m;
            else if (baseCalculo <= 4664.68m)
                return baseCalculo * 0.225m - 651.73m;
            else
                return baseCalculo * 0.275m - 884.96m;
        }

        /// <summary>
        /// Calcula empréstimo pessoal
        /// </summary>
        public EmprestimoPessoalModel CalcularEmprestimoPessoal(EmprestimoPessoalModel modelo)
        {
            var taxaMensal = modelo.TaxaJuros / 100m;
            
            // Cálculo da prestação usando fórmula PMT
            var fatorJuros = (decimal)Math.Pow((double)(1 + taxaMensal), modelo.PrazoMeses);
            modelo.PrestacaoMensal = Math.Round(modelo.ValorEmprestimo * (taxaMensal * fatorJuros) / (fatorJuros - 1), 2);
            
            // Cálculo do IOF (0,38% do valor + 0,0082% por dia)
            var diasIOF = modelo.PrazoMeses * 30; // Aproximação
            modelo.ValorIOF = Math.Round(modelo.ValorEmprestimo * (0.0038m + (0.000082m * diasIOF)), 2);
            
            // Totais
            modelo.ValorTotal = modelo.PrestacaoMensal * modelo.PrazoMeses;
            modelo.TotalJuros = modelo.ValorTotal - modelo.ValorEmprestimo;
            
            // CET (aproximação simples)
            modelo.CET = Math.Round(modelo.TaxaJuros + (modelo.ValorIOF / modelo.ValorEmprestimo * 100 / modelo.PrazoMeses), 2);
            
            modelo.CalculoRealizado = true;
            return modelo;
        }

        /// <summary>
        /// Calcula financiamento de veículo
        /// </summary>
        public FinanciamentoVeiculoModel CalcularFinanciamentoVeiculo(FinanciamentoVeiculoModel modelo)
        {
            modelo.ValorFinanciado = modelo.ValorVeiculo - modelo.ValorEntrada;
            var taxaMensal = modelo.TaxaJuros / 100m / 12m; // Converter anual para mensal
            
            // Cálculo da prestação usando fórmula PMT
            var fatorJuros = (decimal)Math.Pow((double)(1 + taxaMensal), modelo.PrazoParcelas);
            modelo.PrestacaoMensal = Math.Round(modelo.ValorFinanciado * (taxaMensal * fatorJuros) / (fatorJuros - 1), 2);
            
            // Cálculo do IOF
            modelo.ValorIOF = Math.Round(modelo.ValorFinanciado * 0.0038m, 2);
            
            // Totais
            modelo.ValorTotal = modelo.PrestacaoMensal * modelo.PrazoParcelas;
            modelo.TotalJuros = modelo.ValorTotal - modelo.ValorFinanciado;
            
            // CET
            modelo.CET = Math.Round(modelo.TaxaJuros + (modelo.ValorIOF / modelo.ValorFinanciado * 100), 2);
            
            modelo.CalculoRealizado = true;
            return modelo;
        }

        /// <summary>
        /// Calcula hora extra
        /// </summary>
        public HoraExtraModel CalcularHoraExtra(HoraExtraModel modelo)
        {
            // Calcula valor da hora normal
            var horasMes = modelo.JornadaSemanal * 4.33m; // Média de semanas por mês
            modelo.ValorHoraNormal = Math.Round(modelo.SalarioBase / horasMes, 2);
            
            // Calcula horas extras 50%
            modelo.ValorHorasExtras50 = Math.Round(modelo.HorasExtras50 * modelo.ValorHoraNormal * 1.5m, 2);
            
            // Calcula horas extras 100%
            modelo.ValorHorasExtras100 = Math.Round(modelo.HorasExtras100 * modelo.ValorHoraNormal * 2m, 2);
            
            // Calcula adicional noturno (20% sobre hora noturna reduzida)
            var horaNaturnaReduzida = modelo.ValorHoraNormal * (60m / 52.5m); // Hora noturna = 52min30s
            modelo.ValorAdicionalNoturno = Math.Round(modelo.HorasNoturnas * horaNaturnaReduzida * 0.2m, 2);
            
            // Calcula DSR se solicitado
            if (modelo.CalcularDSR)
            {
                var totalExtras = modelo.ValorHorasExtras50 + modelo.ValorHorasExtras100;
                modelo.ValorDSR = Math.Round(totalExtras / 6, 2); // DSR = 1/6 das horas extras
            }
            else
            {
                modelo.ValorDSR = 0;
            }
            
            // Total adicional
            modelo.TotalAdicional = modelo.ValorHorasExtras50 + modelo.ValorHorasExtras100 + 
                                  modelo.ValorAdicionalNoturno + modelo.ValorDSR;
            
            modelo.CalculoRealizado = true;
            return modelo;
        }

        /// <summary>
        /// Calcula 13º salário
        /// </summary>
        public DecimoTerceiroModel CalcularDecimoTerceiro(DecimoTerceiroModel modelo)
        {
            // Salário base para cálculo (inclui médias)
            var salarioBase = modelo.SalarioBruto + modelo.MediaHorasExtras + modelo.MediaComissoes;
            
            // Cálculo do 13º bruto proporcional
            modelo.DecimoTerceiroBruto = Math.Round((salarioBase * modelo.MesesTrabalhados) / 12, 2);
            
            // Cálculo INSS sobre o 13º
            modelo.DescontoINSS = CalcularINSS(modelo.DecimoTerceiroBruto);
            modelo.AliquotaINSS = modelo.DecimoTerceiroBruto > 0 ? Math.Round((modelo.DescontoINSS / modelo.DecimoTerceiroBruto) * 100, 2) : 0;
            
            // Base para IRRF = 13º bruto - INSS - dependentes
            var baseIRRF = modelo.DecimoTerceiroBruto - modelo.DescontoINSS - (modelo.Dependentes * 189.59m);
            
            // Cálculo IRRF
            modelo.DescontoIRRF = Math.Round(Math.Max(0, CalcularIRRF(baseIRRF)), 2);
            modelo.AliquotaIRRF = baseIRRF > 0 ? Math.Round((modelo.DescontoIRRF / baseIRRF) * 100, 2) : 0;
            
            // 13º líquido
            modelo.DecimoTerceiroLiquido = modelo.DecimoTerceiroBruto - modelo.DescontoINSS - modelo.DescontoIRRF;
            
            // Valor a receber (descontando adiantamento)
            modelo.ValorAReceber = Math.Max(0, modelo.DecimoTerceiroLiquido - modelo.Adiantamento);
            
            modelo.CalculoRealizado = true;
            return modelo;
        }

        #endregion
    }
}
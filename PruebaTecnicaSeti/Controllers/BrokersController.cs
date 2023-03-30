using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaSeti.Models;

namespace PruebaTecnicaSeti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokersController : ControllerBase
    {
        private readonly seti_tech_test _context;

        public BrokersController(seti_tech_test context)
        {
            _context = context;
        }

        // GET: api/Brokers
        [HttpGet]
        [Route("ObtenerMejorTiempoRecuperacion")]
        public async Task<ActionResult<IEnumerable<dynamic>>> ObtenerMejorTiempoRecuperacion()
        {
          if (_context.Broker == null)
          {
              return NotFound();
          }
          var resultado = new List<dynamic>();
          var listadoBrokers = await _context.Broker.ToListAsync();
            foreach(var broker in listadoBrokers)
            {
                var proyectos = await _context.InvestmentProject
                    .Where(p => p.BrokerId == broker.BrokerId)
                    .ToListAsync();
                var cuenta = proyectos.Count;
                var tiemposRecuperacion = new List<Decimal>();

                foreach (var p in proyectos)
                {
                    var movimientos = await _context.ProjectMovement
                        .Include(pm => pm.Period)
                        .Where(m => m.ProjectId == p.ProjectId)
                        .OrderBy(m => m.Period.PeriodYear)
                        .ThenBy(m => m.Period.PeriodMonth)
                        .ToListAsync();
                    if (movimientos.Count > 0)
                    {
                        int paso = 0, aniosAntesRetorno = 0;
                        Decimal acumulado = 0;
                        while(acumulado < p.InvestmentAmount 
                            && paso < movimientos.Count)
                        {
                            var flujoRecuperacion = movimientos.Skip(paso).Take(paso + 11)
                                .Select(m => m.MovementAmount).Sum();

                            if(flujoRecuperacion + acumulado > p.InvestmentAmount)
                            {
                                tiemposRecuperacion.Add(aniosAntesRetorno +
                                    ((p.InvestmentAmount - acumulado) / flujoRecuperacion));
                            }
                            
                            acumulado += flujoRecuperacion;
                            aniosAntesRetorno++;
                            paso += 12;
                        }
                    }  
                }
                if (tiemposRecuperacion.Count > 0)
                {
                    resultado.Add(new
                    {
                        Nombre = broker.BrokerName,
                        PromedioTiempoRecuperacion = tiemposRecuperacion.Average()
                    });
                }
            }
            return new List<dynamic> { 
                resultado.OrderBy(broker => broker.PromedioTiempoRecuperacion).FirstOrDefault() 
            };
        }

        // GET: api/Brokers
        [HttpGet]
        [Route("ObtenerMejorBeneficio")]
        public async Task<ActionResult<IEnumerable<dynamic>>> ObtenerMejorBeneficio()
        {
            if (_context.Broker == null)
            {
                return NotFound();
            }
            var resultado = new List<dynamic>();
            var listadoBrokers = await _context.Broker.ToListAsync();
            foreach (var broker in listadoBrokers)
            {
                var proyectos = await _context.InvestmentProject
                    .Where(p => p.BrokerId == broker.BrokerId)
                    .ToListAsync();
                var cuenta = proyectos.Count;
                var beneficioGenerado = new List<Decimal>();

                foreach (var p in proyectos)
                {
                    var movimientos = await _context.ProjectMovement
                        .Include(pm => pm.Period)
                        .ThenInclude(p => p.discountRate)
                        .Where(m => m.ProjectId == p.ProjectId)
                        .OrderBy(m => m.Period.PeriodYear)
                        .ThenBy(m => m.Period.PeriodMonth)
                        .ToListAsync();
                    if (movimientos.Count > 0)
                    {
                        beneficioGenerado.Add(movimientos.Select(m => (m.MovementAmount) / (Decimal)Math.Pow(
                            (1 + ((double)m.Period.discountRate.DiscountRatePercentage / (double)100)), movimientos.IndexOf(m) + 1))
                            .Sum() - p.InvestmentAmount);
                    }
                }
                if (beneficioGenerado.Count > 0)
                {
                    resultado.Add(new
                    {
                        Nombre = broker.BrokerName,
                        PromedioBeneficioGenerado = beneficioGenerado.Average()
                    });
                }
            }
            return new List<dynamic> {
                resultado.OrderByDescending(broker => broker.PromedioBeneficioGenerado).FirstOrDefault()
            };
        }
    }
}

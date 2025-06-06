﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using SafeSpaceAPI.Domain.Entities;
using SafeSpaceAPI.Infrastructure.Context;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SafeSpaceAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SolicitacaoAjudaController : ControllerBase
    {
        private readonly SafeSpaceContext _context;

        public SolicitacaoAjudaController(SafeSpaceContext context)
        {
            _context = context;
        }

        // GET: api/SolicitacaoAjuda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitacaoAjuda>>> GetSolicitacaoAjuda()
        {
            return await _context.SolicitacaoAjuda.ToListAsync();
        }

        // GET: api/SolicitacaoAjuda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitacaoAjuda>> GetSolicitacaoAjuda(Guid id)
        {
            var solicitacaoAjuda = await _context.SolicitacaoAjuda.FromSqlRaw("SELECT * FROM SolicitaçõesAjuda WHERE Id = :p0 AND ROWNUM = 1", id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

            if (solicitacaoAjuda == null)
            {
                return NotFound();
            }

            return solicitacaoAjuda;
        }

        // PUT: api/SolicitacaoAjuda/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitacaoAjuda(Guid id, SolicitacaoAjuda solicitacaoAjuda)
        {
            if (id != solicitacaoAjuda.Id)
            {
                return BadRequest();
            }

            _context.Entry(solicitacaoAjuda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitacaoAjudaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SolicitacaoAjuda
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SolicitacaoAjuda>> PostSolicitacaoAjuda(SolicitacaoAjuda solicitacaoAjuda)
        {
            _context.SolicitacaoAjuda.Add(solicitacaoAjuda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitacaoAjuda", new { id = solicitacaoAjuda.Id }, solicitacaoAjuda);
        }

        // DELETE: api/SolicitacaoAjuda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitacaoAjuda(Guid id)
        {
            // 1. Converter o GUID para formato binário (como o Oracle armazena)
            var idParam = new OracleParameter("p_id", OracleDbType.Raw, 16, id.ToByteArray(), ParameterDirection.Input);

            // 2. Executar consulta usando SQL puro com sintaxe 100% compatível com Oracle 11g
            var solicitacaoAjuda = await _context.SolicitacaoAjuda
                .FromSqlRaw("SELECT * FROM (SELECT * FROM SolicitacoesAjuda WHERE Id = :p_id ORDER BY 1) WHERE ROWNUM = 1", idParam)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (solicitacaoAjuda == null)
            {
                return NotFound();
            }

            // 3. Remoção usando abordagem tradicional do EF Core
            _context.SolicitacaoAjuda.Remove(solicitacaoAjuda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitacaoAjudaExists(Guid id)
        {
            return _context.SolicitacaoAjuda.Any(e => e.Id == id);
        }
    }
}

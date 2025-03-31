using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context; //instancia de AppDbContext como readonly para que não possa ser alterada

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()//também poderia usar List porem IEnumerable é mais otimizado
        // O ActionResult<T> permite que você retorne diferentes tipos de resultados em uma única ação. 
        {
            var produtos = _context.Produtos.ToListAsync(); //acessando a a tebala de produtos e retornando minha lista de produtos

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }
            return await produtos;
        }

        [HttpGet("/primeiro")] //ignora o template de rota definido em Route
        public async Task<ActionResult<Produto>> GetPrimeiro()
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync();
            if(produto is null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado!");
            }

            return produto;

        }

        [HttpPost]
        public ActionResult Post(Produto produto) //antes da versão 2.2 era necessario colocar no parametro da função o [FtomBody]
                                                  //e no corpo era nessario fazer uma validação de !ModelState.IsValid , hoje não é mais necessario
        {
            if (produto is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges(); //comando usado para salvar no banco!

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();//para pessistir as alterações

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //encontrando o produto que será deletado
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado!");
            }

            //caso tenha encontrado vamos remover esse produto do contexto
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}

    

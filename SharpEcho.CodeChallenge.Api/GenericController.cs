using Microsoft.AspNetCore.Mvc;
using SharpEcho.CodeChallenge.Data;
using System.Collections.Generic;

namespace SharpEcho.CodeChallenge.Api
{
    public class GenericController<T> : ControllerBase where T : new()
    {
        protected IRepository Repository;

        public GenericController(IRepository repository)
        {
            Repository = repository;
        }

        // GET: api/<TEntity>
        [HttpGet]
        public virtual IEnumerable<T> Get()
        {
            return Repository.Get<T>();
        }

        // GET: api/<TEntity>/5
        [HttpGet("{id}")]
        public virtual ActionResult<T> Get(long id)
        {
            var entity = Repository.Get<T>(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        // PUT: api/<TEntity>/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public virtual ActionResult<T> Put(long id, T entity)
        {
            var entityToUpdate = entity as dynamic;
            entityToUpdate = Repository.Get<T>(entityToUpdate.Id);

            if (entityToUpdate == null)
            {
                return NotFound();
            }

            if (entityToUpdate.Id != id)
            {
                return BadRequest();
            }

            Repository.Update(entity);
            return entity;
        }

        // POST: api/<TEntity>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public virtual ActionResult<T> Post(T entity)
        {
            var id = Repository.Insert(entity);
            var updatedEntity = entity as dynamic;
            updatedEntity.Id = id;

            return Created(entity.GetType().Name + '/' + id.ToString(), updatedEntity);
        }

        // DELETE: api/<TEntity>/5
        [HttpDelete("{id}")]
        public virtual ActionResult<T> Delete(long id)
        {
            var entity = Repository.Get<T>(id);
            if (entity == null)
            {
                return NotFound();
            }

            Repository.Delete<T>(id);
            return entity;
        }
    }
}

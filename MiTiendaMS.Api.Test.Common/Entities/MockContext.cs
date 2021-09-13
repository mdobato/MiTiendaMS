using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MiTiendaMS.Api.Test.Common.Entities
{
    public class MockContext<C, M> where C : class
                                   where M : class
    {
        private Mock<DbSet<M>> _model { get; set; }
        private IQueryable<M> _data { get; set; }

        public MockContext(IQueryable<M> data)
        {
            this._data = data;
            this._model = new Mock<DbSet<M>>();
        }

        public Mock<C> GetDataContext(Expression<Func<C,DbSet<M>>> exp)
        {
            this._model.As<IQueryable<M>>().Setup(x => x.Provider).Returns(this._data.Provider);
            this._model.As<IQueryable<M>>().Setup(x => x.Expression).Returns(this._data.Expression);
            this._model.As<IQueryable<M>>().Setup(x => x.ElementType).Returns(this._data.ElementType);
            this._model.As<IQueryable<M>>().Setup(x => x.GetEnumerator()).Returns(this._data.GetEnumerator());
            this._model.As<IAsyncEnumerable<M>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<M>(this._data.GetEnumerator()));
            this._model.As<IQueryable<M>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<M>(this._data.Provider));

            var context = new Mock<C>();
            context.Setup(exp).Returns(this._model.Object);
            return context;

        }
    }
}

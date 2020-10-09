using CoxAutomotive.Models.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoxAutomotive.Services
{
    public interface IPathService
    {
        string GetPathFor<TOut>(params object[] args);
    }

    public class PathService : IPathService
    {
        private readonly IDictionary<Type, string> _paths;

        public PathService(IEnumerable<IPath> paths)
        {
            _paths = paths.ToDictionary(path => path.Type, path => path.Template);
        }

        public string GetPathFor<TOut>(params object[] args)
        {
            if(_paths.TryGetValue(typeof(TOut), out var template))
            {
                return string.Format(template, args);
            }
            throw new ArgumentException(typeof(TOut).Name);
        }
    }
}

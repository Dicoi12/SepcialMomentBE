using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SepcialMomentBE.Models;
using SepcialMomentBE.Data;
using System.Linq.Expressions;

namespace SepcialMomentBE.Services
{
    public class GenericCrudService<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericCrudService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<ServiceResult<List<TEntity>>> GetAllAsync()
        {
            var result = new ServiceResult<List<TEntity>>();
            result.Result = await _dbSet.ToListAsync();
            return result;
        }

        public async Task<ServiceResult<TEntity?>> GetByIdAsync(object id)
        {
            var result = new ServiceResult<TEntity?>();
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                result.AddValidationMessage($"Nu s-a găsit entitatea cu id-ul {id}");
            }
            result.Result = entity;
            return result;
        }

        public async Task<ServiceResult<List<TEntity>>> GetByPropertyAsync(string propertyName, string value)
        {
            var result = new ServiceResult<List<TEntity>>();
            var property = typeof(TEntity).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                result.AddValidationMessage($"Proprietatea {propertyName} nu există în entitatea {typeof(TEntity).Name}");
                return result;
            }

            try
            {
                object convertedValue = value;
                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    if (int.TryParse(value, out int intValue))
                    {
                        convertedValue = intValue;
                    }
                    else
                    {
                        result.AddValidationMessage($"Valoarea {value} nu poate fi convertită la număr întreg");
                        return result;
                    }
                }
                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    if (decimal.TryParse(value, out decimal decimalValue))
                    {
                        convertedValue = decimalValue;
                    }
                    else
                    {
                        result.AddValidationMessage($"Valoarea {value} nu poate fi convertită la număr zecimal");
                        return result;
                    }
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(value, out DateTime dateValue))
                    {
                        convertedValue = dateValue;
                    }
                    else
                    {
                        result.AddValidationMessage($"Valoarea {value} nu poate fi convertită la dată");
                        return result;
                    }
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    if (bool.TryParse(value, out bool boolValue))
                    {
                        convertedValue = boolValue;
                    }
                    else
                    {
                        result.AddValidationMessage($"Valoarea {value} nu poate fi convertită la boolean");
                        return result;
                    }
                }
                else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
                {
                    if (Guid.TryParse(value, out Guid guidValue))
                    {
                        convertedValue = guidValue;
                    }
                    else
                    {
                        result.AddValidationMessage($"Valoarea {value} nu poate fi convertită la GUID");
                        return result;
                    }
                }

                var parameter = Expression.Parameter(typeof(TEntity), "x");
                var propertyExpression = Expression.Property(parameter, property);
                var constantExpression = Expression.Constant(convertedValue);
                var equalExpression = Expression.Equal(propertyExpression, constantExpression);
                var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);

                result.Result = await _dbSet.Where(lambda).ToListAsync();
            }
            catch (Exception ex)
            {
                result.AddValidationMessage($"Eroare la căutarea după proprietatea {propertyName}: {ex.Message}");
            }

            return result;
        }

        public async Task<ServiceResult<TEntity>> CreateAsync(TEntity entity)
        {
            var result = new ServiceResult<TEntity>();
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            result.Result = entity;
            return result;
        }

        public async Task<ServiceResult<TEntity?>> UpdateAsync(object id, TEntity entity)
        {
            var result = new ServiceResult<TEntity?>();
            var existing = await _dbSet.FindAsync(id);
            if (existing == null)
            {
                result.AddValidationMessage($"Nu s-a găsit entitatea cu id-ul {id}");
                return result;
            }
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            result.Result = entity;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAsync(object id)
        {
            var result = new ServiceResult<bool>();
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                result.AddValidationMessage($"Nu s-a găsit entitatea cu id-ul {id}");
                result.Result = false;
                return result;
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            result.Result = true;
            return result;
        }
    }
} 
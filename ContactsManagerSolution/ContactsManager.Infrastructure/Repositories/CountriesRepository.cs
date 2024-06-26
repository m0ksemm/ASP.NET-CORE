﻿using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using RepositoryContracts;
using System;

namespace Repositories
{
    public class CountriesRepository : RepositoryContracts.ICountriesRepository
    {
        private readonly Entities.ApplicationDbContext _db;

        public CountriesRepository(Entities.ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country); 
            await _db.SaveChangesAsync();

            return country;
        }



        public async Task<List<Country>> GetAllCountries()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryId(Guid countryID)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            return await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }

        public async Task<bool> DeleteCountry(Guid countryID)
        {
            _db.Countries.RemoveRange(_db.Countries.Where(temp => temp.CountryID == countryID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<Country> UpdateCountry(Country country)
        {
            Country? matchingCountry = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == country.CountryID);
            if (matchingCountry == null)
            {
                return country;
            }
            matchingCountry.CountryName = country.CountryName;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingCountry;
        }
    }
}

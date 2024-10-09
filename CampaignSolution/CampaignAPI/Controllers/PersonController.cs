﻿using CampaignService.Models;
using CampaignService.Services;
using Microsoft.AspNetCore.Mvc;
using CampaignService.Interfaces;
using CampaignService.Constants;
using System.Diagnostics.Metrics;
using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CampaignAPI.Controllers
{
    public class PersonController : Controller
    {
        private readonly ISoapService _soapService;
        public PersonController(ISoapService soapService)
        {
            _soapService = soapService;
        }

        [HttpGet("get_customer/{id:int}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (id >= Settings.CUSTOMER_ID_MIN && id <= Settings.CUSTOMER_ID_MAX)
            {
                var customer = await _soapService.FindPersonById<Customer>(id);

                return customer != null ? StatusCode(StatusCodes.Status200OK, (customer)) : StatusCode((int)StatusCodes.Status502BadGateway, "We're experiencing technical difficulties. Please try again later or contact support if the issue persists.");
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"The requested ID is out of the allowed range.Please enter an ID between {Settings.CUSTOMER_ID_MIN} and {Settings.CUSTOMER_ID_MAX}.");
            }
           
        }

        [HttpGet("get_people", Name = "GetPeople")]
        [ProducesResponseType(typeof(List<Person>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            try
            {
                List<Person> people = await _soapService.GetPeople();

                if (people == null || !people.Any())
                {
                    return StatusCode(StatusCodes.Status404NotFound, "There are no available data for people.");
                }
                return StatusCode(StatusCodes.Status200OK, people);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}

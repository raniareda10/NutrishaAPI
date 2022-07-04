using System.Collections.Generic;
using System.Threading.Tasks;
using DL.DtosV1.UserMeasurements;
using DL.EntitiesV1.Measurements;
using DL.Repositories.UserMeasurement;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class UserMeasurementController : BaseMobileController
    {
        private readonly UserMeasurementRepository _userMeasurementRepository;

        public UserMeasurementController(UserMeasurementRepository userMeasurementRepository)
        {
            _userMeasurementRepository = userMeasurementRepository;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] PostUserMeasurement postUserMeasurement)
        {
            if (postUserMeasurement.MeasurementValue <= 0)
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            
            await _userMeasurementRepository.PostAsync(postUserMeasurement);
            return EmptyResult();
        }

        [HttpPost("PostMultiMeasurements")]
        public async Task<IActionResult> PostMultiMeasurementsAsync([FromBody] IList<PostUserMeasurement> measurements)
        {
            await _userMeasurementRepository.PostMultiMeasurementsAsync(measurements);
            return EmptyResult();
        }

        [HttpGet("GetLastMeasurementDetails")]
        public async Task<IActionResult> GetLastMeasurementDetailsAsync(MeasurementType measurementType)
        {
            return ItemResult(await _userMeasurementRepository.GetLastMeasurementDetailsAsync(measurementType));
        }

        [HttpGet("GetMeasurements")]
        public async Task<IActionResult> GetMeasurementsListAsync([FromQuery] IList<MeasurementType> measurementTypes)
        {
            if (measurementTypes == null || measurementTypes?.Count == 0)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            return ListResult(await _userMeasurementRepository.GetMeasurementsListAsync(measurementTypes));
        }

        [HttpGet("GetMeasurementsForOneType")]
        public async Task<IActionResult> GetMeasurementsForOneTypeOnlyAsync(MeasurementType measurementType)
        {
            return ItemResult(await _userMeasurementRepository.GetMeasurementsListForOneTypeOnlyAsync(measurementType));
        }
    }
}
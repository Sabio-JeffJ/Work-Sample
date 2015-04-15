using Microsoft.AspNet.Identity.EntityFramework;
using Sabio.Web.Models;
using Sabio.Web.Exceptions;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Models.ViewModels;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Web.Http;
using SendGrid;
using Sabio.Web.Domain.Users;
using Sabio.Web.Domain;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/contentsites")]
    public class ContentSitesApiController : ApiController
    {
        [Route("profile/photo"), HttpPut, Authorize]
        public HttpResponseMessage UpdateContentSiteImage(ContentSitesAddRequest model)
        {
            if (!ModelState.IsValid)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = ContentSitesService.GetCurrentUserId();

            ContentSitesService.UpdatePhoto(userId, model.Photo);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route, HttpPost, Authorize]
        public HttpResponseMessage AddInfo(ContentSitesAddRequest model)
        {
            if (ModelState.IsValid)
            {
                ItemResponse<Guid> response = new ItemResponse<Guid>();


                Int32 appUserId = UserService.GetAppUserId(UserService.GetCurrentUserId());


                response.Item = ContentSitesService.InsertNewContent(model.Name, model.Url, model.PublishDate, model.ExpireDate, model.Photo, appUserId);


                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("details/{ContentSiteUid:guid}"), HttpPut, Authorize]
        public HttpResponseMessage UpdateContentDetails(ContentSitesUpdateRequest model, Guid ContentSiteUid)
        {
            if (!ModelState.IsValid)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                int AppUserId = UserService.GetAppUserId(UserService.GetCurrentUserId());

                ContentSitesService.UpdateContentSitesDetails(model, AppUserId);

                SuccessResponse response = new SuccessResponse();

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

        }

        [Route("{ContentPageUid:guid}"), HttpGet]
        public HttpResponseMessage RetrieveDetails(Guid ContentPageUid)
        {

            ItemResponse<ContentSitesDetails> response = new ItemResponse<ContentSitesDetails>();

            response.Item = ContentSitesService.GetContentDetails(ContentPageUid);

            return Request.CreateResponse(response);
        }

        [Route("edit/{ContentPageUid:guid}"), HttpDelete]
        public HttpResponseMessage DeletePage(Guid ContentPageUid)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ContentSitesService.DeletePage(ContentPageUid);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }



}


using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using System.Data;
using Sabio.Web.Domain;
using Sabio.Data;



namespace Sabio.Web.Services
{
    public class ContentSitesService : BaseService
    {


        public static string GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }


        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetCurrentUserId());

        }

        public static void UpdatePhoto(string id, string photo)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ContentSites_Update_Photo"
               , inputParamMapper: delegate(SqlParameterCollection paramCollection)
               {


                   paramCollection.AddWithValue("@Id", id);
                   paramCollection.AddWithValue("@Photo", photo);



               }, returnParameters: delegate(SqlParameterCollection paramCollection)
               {

               });
        }

        public static ContentSitesAddRequest GetUserPhoto(string userId)
        {
            ContentSitesAddRequest PhotoValue = null;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ContentSites_SelectPhotoByUserId"
           , inputParamMapper: delegate(SqlParameterCollection paramCollection)
           {
               paramCollection.AddWithValue("@Id", userId);

               SqlParameter photo = new SqlParameter("@Photo", System.Data.SqlDbType.NVarChar);
               photo.Direction = System.Data.ParameterDirection.Output;
               photo.Size = 200;

               paramCollection.Add(photo);


           }, returnParameters: delegate(SqlParameterCollection param)
           {

               PhotoValue = new ContentSitesAddRequest();
               PhotoValue.Photo = param["@Photo"].Value.ToString();

           }
          );

            return PhotoValue;

        }


        public static Guid InsertNewContent(string Name, string Url, DateTime PublishDate, DateTime ExpireDate, string Photo, int appUserId)
        {
            Guid uid = Guid.Empty;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ContentSites_Insert"
                , inputParamMapper: delegate(SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Name", Name);
                    paramCollection.AddWithValue("@URL", Url);
                    paramCollection.AddWithValue("@PublishDate", PublishDate);
                    paramCollection.AddWithValue("@ExpireDate", ExpireDate);
                    paramCollection.AddWithValue("@Photo", Photo);
                    paramCollection.AddWithValue("@AppUserId", appUserId);

                    SqlParameter u = new SqlParameter("@Uid", System.Data.SqlDbType.UniqueIdentifier);
                    u.Direction = System.Data.ParameterDirection.Output;

                    paramCollection.Add(u);

                }, returnParameters: delegate(SqlParameterCollection param)
                {
                    Guid.TryParse(param["@Uid"].Value.ToString(), out uid);
                });

            return uid;


        }

        public static void UpdateContentSitesDetails(ContentSitesUpdateRequest model, int AppUserId)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ContentSites_Update"
               , inputParamMapper: delegate(SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ContentSiteUid", model.ContentSiteUid);

                   paramCollection.AddWithValue("@AppUserId", AppUserId);

                   paramCollection.AddWithValue("@Name", model.Name);

                   paramCollection.AddWithValue("@URL", model.Url);

                   paramCollection.AddWithValue("@PublishDate", model.PublishDate);

                   paramCollection.AddWithValue("@ExpireDate", model.ExpireDate);

                   paramCollection.AddWithValue("@Photo", model.Photo);

               });

        }

        public static ContentSitesDetails GetContentDetails(Guid ContentPageUid)
        {
            ContentSitesDetails details = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.ContentSites_GetByContentSiteUid"
           , inputParamMapper: delegate(SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@UID", ContentPageUid);  

              }
      , map: delegate(IDataReader reader, short set)
      {
          details = new ContentSitesDetails();
          int startingIndex = 0; //startingOrdinal

          details.ContentPageUid = reader.GetGuid(startingIndex++);
          details.Name = reader.GetSafeString(startingIndex++);
          details.URL = reader.GetSafeString(startingIndex++);
          details.PublishDate = reader.GetSafeDateTime(startingIndex++);
          details.ExpireDate = reader.GetSafeDateTime(startingIndex++);
          details.Photo = reader.GetSafeString(startingIndex++);

      }
      );

            return details;
        }

        public static void DeletePage(Guid ContentPageUid)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.ContentSites_DeleteByContentSiteUid"
            , inputParamMapper: delegate(SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@UID", ContentPageUid);
            }
            );
            // returnParameters: null;
        }



    }
}


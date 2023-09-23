using LiteDB;
using Microsoft.AspNetCore.Mvc;
using WebVRChatOSC.DTO;

namespace WebVRChatOSC.API
{
    [ApiController]
    [Route("api/button")]
    public class ButtonAPIController : ControllerBase
    {
        [HttpGet("search")]
        public IEnumerable<ButtonData> SearchButtons(ILiteDatabase db, int? category)
        {
            var collection = db.GetCollection<ButtonData>("button");
            var query = collection.Query();
            if (category != null)
                query = query.Where(x => x.category == category);
            return query.ToList();
        }

        [HttpPost("add")]
        public void AddButton(ILiteDatabase db, ButtonData button)
        {
            if (string.IsNullOrWhiteSpace(button.color))
                button.color = null;
            if (string.IsNullOrWhiteSpace(button.icon))
                button.icon = null;
            if (string.IsNullOrWhiteSpace(button.action))
                button.action = null;
            var collection = db.GetCollection<ButtonData>("button");
            collection.Insert(button);
        }

        [HttpPost("add-many")]
        public void AddManyButton(ILiteDatabase db, ButtonData[] buttons)
        {
            foreach (var button in buttons)
            {
                AddButton(db, button);
            }
        }

        [HttpPost("update")]
        public void Update(ILiteDatabase db, ButtonData button)
        {
            var collection = db.GetCollection<ButtonData>("button");
            collection.Update(button.id, button);
        }

        [HttpPost("delete")]
        public void DeleteButton(ILiteDatabase db, int id)
        {
            var collection = db.GetCollection<ButtonData>("button");
            collection.Delete(id);
        }

        [HttpPost("delete-many")]
        public void DeleteButton(ILiteDatabase db, int[] ids)
        {
            var collection = db.GetCollection<ButtonData>("button");
            foreach (var id in ids)
                collection.Delete(id);
        }
    }
}

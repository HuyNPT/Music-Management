using ASS_PRC.Data.UnitOfWork;
using ASS_PRC.Services.DTO;
using ASS_PRC.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASS_PRC.Services.Services
{
    public class PlaylistService : IPlaylistService
    {
        private const int PageLimit = 15;
        private readonly IUnitOfWork _unitOfWork;

        public PlaylistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> DeletePlayListWebAdmin(Guid PlaylistId)
        {
            var playlist = _unitOfWork.Repository<Data.Entity.Playlist>().Find(x => x.Id == PlaylistId);
            if(playlist == null)
            {
                return "Unauthorized";
            }           
            playlist.IsDelete = true;
            try
            {
                _unitOfWork.Commit();
                return "Delete Successfully";
            }catch(Exception e)
            {
                return "Delete fail";
            }
           
        }

       

        public async Task<int> GetPlaylistCount()
        {
            var count =_unitOfWork.Repository<Data.Entity.Playlist>().FindAll(x => x.IsDelete == false).Count();
            return count;
        }

        public async Task<IList<PlaylistWebAdmin>> GetPlayListWebAdmin(Enum SortType, bool isPaging, int page, int pageLimitItem, string searchKey)
        {
            List<PlaylistWebAdmin> listPlaylist = new List<PlaylistWebAdmin>();
            var playlistEntities = _unitOfWork.Repository<Data.Entity.Playlist>().GetAll().Where(p => p.IsDelete == false &&p.PlaylistName.StartsWith(searchKey == null ? "" : searchKey))
            .Include(a => a.CategoryPlaylist).ThenInclude(d => d.Category).AsQueryable();
            if(playlistEntities.ToList().Count == 0)
            {
                return listPlaylist;
            }
            int sortTypeInt = -1;
            try
            {
                sortTypeInt = int.Parse(SortType.ToString());
            }
            catch (Exception e)
            {

            }


            if (sortTypeInt != 0)
            {
                if (SortType.ToString().Equals("SortDesc"))
                {
                    playlistEntities = playlistEntities.OrderByDescending(x => x.PlaylistName);
                }
                else
                {
                    playlistEntities = playlistEntities.OrderBy(x => x.PlaylistName);
                }
            }
            if (isPaging)
            {
                playlistEntities = playlistEntities.Skip(page * pageLimitItem).Take(pageLimitItem);
            }
            foreach (var item in playlistEntities)
            {
                List<DTO.CategoryPlaylist> listCategoryPlaylist = new List<DTO.CategoryPlaylist>();
                item.CategoryPlaylist.ToList().ForEach(e =>
                {
                    DTO.CategoryPlaylist tmp = new DTO.CategoryPlaylist();
                    tmp.Id = e.Id;
                    tmp.PlaylistId = e.PlaylistId;
                    tmp.CategoryId = e.CategoryId;
                    tmp.Category = new List<DTO.Category>()
                        {
                            new DTO.Category()
                            {
                                Id=e.Category.Id,
                                CategoryName=e.Category.CategoryName
                            }
                        };
                    listCategoryPlaylist.Add(tmp);
                }
                );
                listPlaylist.Add(new DTO.PlaylistWebAdmin()
                {
                    Id = item.Id,
                    PlaylistName = item.PlaylistName,
                    CreateDate = item.CreateDate,                   
                    ImageUrl=item.ImageUrl,
                    IsDelete=item.IsDelete,
                    CategoryPlaylists = listCategoryPlaylist
                });
            }
            return listPlaylist;
        }
     

        public async Task<bool> PostPlayListWebAdmin(string Name, string ImageUrl, List<Guid> Category, Guid UserId)
        {
            Guid playlistID = Guid.NewGuid();
            _unitOfWork.Repository<Data.Entity.Playlist>().Insert(
                new Data.Entity.Playlist()
                {
                    Id = playlistID,
                    PlaylistName = Name,
                    CreateBy = UserId,
                    CreateDate = GetTimeNowVN.GetTimeNowVietNam(),                  
                    ImageUrl = ImageUrl,
                    IsDelete = false,                                     
                }
               );
            _unitOfWork.Commit();
            var catePlaylist = _unitOfWork.Repository<Data.Entity.CategoryPlaylist>();
            Category.ForEach(e =>
            {
                catePlaylist.Insert(new Data.Entity.CategoryPlaylist()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = e,
                    PlaylistId = playlistID
                }
                );
            });
            try
            {
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                return false;
            }
            

            return true;
        }

       
        public async Task<PlaylistWebAdmin> GetPlaylistById(Guid PlaylistId)
        {
            var rs =_unitOfWork.Repository<Data.Entity.Playlist>().GetAll().Where(p => p.IsDelete == false & p.Id == PlaylistId)
            .Include(a => a.CategoryPlaylist).ThenInclude(d => d.Category).FirstOrDefault();
            if(rs == null)
            {
                return null;
            }
            List<DTO.CategoryPlaylist> listCategoryPlaylist = new List<DTO.CategoryPlaylist>();
            rs.CategoryPlaylist.ToList().ForEach(e =>
            {
                DTO.CategoryPlaylist tmp = new DTO.CategoryPlaylist();
                tmp.Id = e.Id;
                tmp.PlaylistId = e.PlaylistId;
                tmp.CategoryId = e.CategoryId;
                tmp.Category = new List<DTO.Category>()
                        {
                            new DTO.Category()
                            {
                                Id=e.Category.Id,
                                CategoryName=e.Category.CategoryName
                            }
                        };
                listCategoryPlaylist.Add(tmp);
            }
            );
            PlaylistWebAdmin x = new PlaylistWebAdmin()
            {
                Id = rs.Id,
                PlaylistName = rs.PlaylistName,
                CreateDate = rs.CreateDate,
                ImageUrl = rs.ImageUrl,
                IsDelete = rs.IsDelete,               
                CategoryPlaylists = listCategoryPlaylist

            };
            return x;
        }

        public async Task<string> PutPlayListWebAdmin(Guid PlaylistId, string Name, string ImageUrl, List<Guid> Category, Guid UserId)
        {
            var query = _unitOfWork.Repository<Data.Entity.Playlist>().GetById(PlaylistId);            
            if(query.IsDelete == true)
            {
                return "Fail";
            }
            query.PlaylistName = Name;      
            query.ImageUrl = ImageUrl;           

            var categoryPl = _unitOfWork.Repository<Data.Entity.CategoryPlaylist>().FindAll(x => x.PlaylistId == PlaylistId);
            foreach (var item in categoryPl)
            {
                _unitOfWork.Repository<Data.Entity.CategoryPlaylist>().HardDelete(item.Id);
            }

            var catePlaylist = _unitOfWork.Repository<Data.Entity.CategoryPlaylist>();
            Category.ForEach(e =>
            {
                catePlaylist.Insert(new Data.Entity.CategoryPlaylist()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = e,
                    PlaylistId = PlaylistId
                }
                );
            });
            
            try
            {
                _unitOfWork.Commit();
                return "Success";
            }
            catch (Exception e)
            {
                return "Fail";
            }
        }
    }
}

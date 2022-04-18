using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StackOverload.Data;
using StackOverload.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StackOverload.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

         // GET
        public async Task<IActionResult> Index(string? count, int pageNumber=1)
        {
            try
            {
                var AllQuestions = _db.Questions.Include(q => q.Answers)
                                                .Include(q => q.User)
                                                .Include(q => q.TagQuestions).ThenInclude(q => q.Tag);

                var SortedByDate = AllQuestions.OrderByDescending(q => q.PostDate);
                PaginatedList<Question> questionList = await PaginatedList<Question>.CreateAsync(SortedByDate, pageNumber, 10);
                
                if (count != null)
                {
                    var SortedByCount = AllQuestions.OrderByDescending(q => q.Answers.Count());
                    PaginatedList<Question> questionList2 = await PaginatedList<Question>.CreateAsync(SortedByCount, pageNumber, 10);
                    return View(questionList2);
                }

                ViewBag.CurrentUserName = User.Identity.Name;

                return View(questionList);

            }
            catch
            {
                return NotFound();
            }
            
        }

        // GET
        public async Task<IActionResult> QuestionDetails(int questionId, string? message)
        {
            try
            {
                var getCurrentUser = User.Identity.Name;

                if(getCurrentUser != null)
                {
                    ApplicationUser currentUser = await _userManager.FindByNameAsync(getCurrentUser);
                    ViewBag.CurrentUser = currentUser;
                }

                Question currentQuestion = _db.Questions.Include(q => q.Answers).ThenInclude(q => q.User)
                                                        .Include(q => q.Comments)
                                                        .Include(q => q.User)
                                                        .Include(q => q.Votes)
                                                        .Include(q => q.TagQuestions).ThenInclude(q => q.Tag)
                                                        .First(q => q.Id == questionId);

                List<Answer> answersList = _db.Answers.Include(a => a.Comments)
                                                      .Include(a => a.Votes)
                                                      .Where(a => a.QuestionId == questionId).ToList();

                List<TagQuestion> tagsList = currentQuestion.TagQuestions.ToList();
                List<Comment> answerCommentList = currentQuestion.Comments.ToList();

                ViewBag.CurrentQuestion = currentQuestion;
                ViewBag.Tags = tagsList;
                ViewBag.Message = message;

                return View(answersList.OrderByDescending(a => a.IsCorrect));
            }
            catch
            {
                return NotFound();
            }
        }

        // GET
        public IActionResult TagSearch(int? id)
        {
            try
            {
                ViewBag.CurrentUserName = User.Identity.Name;

                ViewBag.Tag = _db.Tags.First(t => t.Id == id);
                List<TagQuestion> QuestionList = _db.TagQuestions.Include(t => t.Tag)
                                                                  .Include(q => q.Question).ThenInclude(q => q.User)
                                                                  .Include(q => q.Question).ThenInclude(q => q.Answers)
                                                                  .Where(t => t.Tag.Id == id).ToList();

                return View(QuestionList.OrderByDescending(q => q.Question.PostDate));
            }
            catch
            {
                return NotFound();
            }

        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AnswerVote(string user, int? questionId, int? answerId, string? voteType)
        {
            if(user != null && questionId != null && voteType != null && answerId != null)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.FindByIdAsync(user);
                    Question currentQuestion = _db.Questions.First(q => q.Id == questionId);
                    Answer currentAnswer = _db.Answers.Include(a => a.User).First(a => a.Id == answerId);

                    if (voteType == "upVote")
                    {

                        Vote newVote = new Vote()
                        {
                            User = currentUser,
                            Answer = currentAnswer,
                            Voted = true,
                        };

                        currentAnswer.User.Reputation += 5;
                        _db.Votes.Add(newVote);
                        _db.SaveChanges();
                        return RedirectToAction("QuestionDetails", new { questionId = currentQuestion.Id, message = "Thanks for voting!" });
                    }

                    if (voteType == "downVote")
                    {
                        Vote newVote = new Vote()
                        {
                            User = currentUser,
                            Answer = currentAnswer,
                            Voted = false,
                        };

                        currentAnswer.User.Reputation -= 5;
                        _db.Votes.Add(newVote);
                        _db.SaveChanges();
                        return RedirectToAction("QuestionDetails", new { questionId = currentQuestion.Id, message = "Thanks for voting!" });
                    }
                }
                catch
                {
                    return BadRequest();
                }   
            }
            return BadRequest();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> QuestionVote(string user, int? questionId, string? voteType)
        {
            if (user != null && questionId != null && voteType != null)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.FindByIdAsync(user);
                    Question currentQuestion = _db.Questions.Include(q => q.User).First(q => q.Id == questionId);

                    if (voteType == "upVote")
                    {
                        Vote newVote = new Vote()
                        {
                            User = currentUser,
                            Question = currentQuestion,
                            Voted = true,
                        };

                        currentQuestion.User.Reputation += 5;
                        _db.Votes.Add(newVote);
                        _db.SaveChanges();
                        return RedirectToAction("QuestionDetails", new { questionId = currentQuestion.Id, message = "Thanks for voting!" });
                    }

                    if (voteType == "downVote")
                    {
                        Vote newVote = new Vote()
                        {
                            User = currentUser,
                            Question = currentQuestion,
                            Voted = false,
                        };

                        currentQuestion.User.Reputation -= 5;
                        _db.Votes.Add(newVote);
                        _db.SaveChanges();
                        return RedirectToAction("QuestionDetails", new { questionId = currentQuestion.Id, message = "Thanks for voting!" });
                    }
                }
                catch
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SelectAnswer(int answerId, int questionId, string userId)
        {
            Answer answer = _db.Answers.First(a => a.Id == answerId);
            Question question = _db.Questions.First(q => q.Id == questionId);
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            question.IsAnswered = true;
            answer.IsCorrect = true;
            _db.SaveChanges();

            return RedirectToAction("QuestionDetails", new { questionId = question.Id});
        }


        // GET
        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex(string? date, string? count)
        {
            List<Question> questionList = _db.Questions.Include(q => q.Answers)
                                                       .Include(q => q.User)
                                                       .Include(q => q.TagQuestions).ThenInclude(q => q.Tag)
                                                       .ToList();

            if (count != null)
            {
                return View(questionList.OrderByDescending(q => q.Answers.Count()));
            }

            ViewBag.CurrentUser = User.Identity.Name;
            return View(questionList.OrderByDescending(q => q.PostDate));
        }

        [HttpPost]
        public IActionResult AdminIndex(int questionId)
        {
            Question question = _db.Questions.Include(q => q.Comments)
                                             .Include(q => q.TagQuestions)
                                             .Include(q => q.Answers).ThenInclude(q => q.Comments)
                                             .First(q => q.Id == questionId);

            _db.Questions.Remove(question);
            _db.SaveChanges();

            return RedirectToAction("AdminIndex");
        }


        // GET
        [Authorize]
        public async Task<IActionResult> PostQuestion()
        {
            var CurrentUserName = User.Identity.Name;
            ApplicationUser currentUser = await _userManager.FindByNameAsync(CurrentUserName);

            ViewBag.UserId = currentUser.Id;
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostQuestion(string userId, string title, string body, int id1, int id2, int id3)
        {
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            Tag tag1 = _db.Tags.First(t => t.Id == id1);
            Tag tag2 = _db.Tags.First(t => t.Id == id2);
            Tag tag3 = _db.Tags.First(t => t.Id == id3);

            if (id1 == id2 || id1 == id3 || id2 == id3)
            {
                ViewBag.Title = title;
                ViewBag.Body = body;
                ViewBag.Error = "Duplicate Tags were selected, please ensure each tag is different.";
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name");

                return View("PostQuestion");
            }

            if (currentUser != null)

            {
                Question newQuestion = new Question()
                {
                    Title = title,
                    Body = body,
                    User = currentUser,
                    PostDate = DateTime.Now,
                };

                List<TagQuestion> newTags = new List<TagQuestion>()
                {
                    new TagQuestion(tag1, newQuestion),
                    new TagQuestion(tag2, newQuestion),
                    new TagQuestion(tag3, newQuestion)
                };

                _db.Questions.Add(newQuestion);
                _db.TagQuestions.AddRange(newTags);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }


        // GET
        [Authorize]
        public async Task<IActionResult> PostAnswer(int questionId, string userName)
        {
            ApplicationUser currentUser = await _userManager.FindByNameAsync(userName);
            Question currentQuestion = _db.Questions.First(q => q.Id == questionId);

            ViewBag.UserId = currentUser.Id;
            ViewBag.QuestionId = currentQuestion.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostAnswer(string userId, int questionId, string body)
        {
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            Question currentQuestion = _db.Questions.First(q => q.Id == questionId);

            if (currentUser != null && currentQuestion != null)
            {
                Answer newAnswer = new Answer()
                {
                    Body = body,
                    User = currentUser,
                    Question = currentQuestion,
                    PostDate = DateTime.Now,
                };

                _db.Answers.Add(newAnswer);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }


        // GET
        [Authorize]
        public async Task<IActionResult> PostComment(int? questionId, int? answerId, string? user)
        {
            ApplicationUser currentUser = await _userManager.FindByIdAsync(user);
            ViewBag.UserId = currentUser.Id;

            if (questionId != null)
            {
                Question currentQuestion = _db.Questions.First(q => q.Id == questionId);
                ViewBag.QuestionId = currentQuestion.Id;
            }

            if (answerId != null)
            {
                Answer currentAnswer = _db.Answers.First(a => a.Id == answerId);
                ViewBag.AnswerId = currentAnswer.Id;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(string userId, int? questionId, int? answerId, string body)
        {
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser != null && questionId != null)
            {
                Question currentQuestion = _db.Questions.First(q => q.Id == questionId);
                Comment newComment = new Comment()
                {
                    Body = body,
                    User = currentUser,
                    Question = currentQuestion,
                    PostDate = DateTime.Now,
                };

                _db.Comments.Add(newComment);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            if (currentUser != null && answerId != null)
            {
                Answer currentAnswer = _db.Answers.First(a => a.Id == answerId);
                Comment newComment = new Comment()
                {
                    Body = body,
                    User = currentUser,
                    Answer = currentAnswer,
                    PostDate = DateTime.Now,
                };

                _db.Comments.Add(newComment);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }


        





























        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
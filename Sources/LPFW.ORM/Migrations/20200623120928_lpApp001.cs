using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LPFW.ORM.Migrations
{
    public partial class lpApp001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationMenuGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    PortalUrl = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationMenuGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCommentTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    RefrenceCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCommentTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    RefrenceCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 200, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: true),
                    ParentRoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_AspNetRoles_ParentRoleId",
                        column: x => x.ParentRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 200, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Sex = table.Column<bool>(nullable: true),
                    Birthdays = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    AttachmentTimeUploaded = table.Column<DateTime>(nullable: false),
                    OriginalFileName = table.Column<string>(maxLength: 500, nullable: true),
                    UploadPath = table.Column<string>(maxLength: 500, nullable: true),
                    IsInDB = table.Column<bool>(nullable: false),
                    UploadFileSuffix = table.Column<string>(maxLength: 10, nullable: true),
                    BinaryContent = table.Column<byte[]>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    IconString = table.Column<string>(maxLength: 120, nullable: true),
                    IsUnique = table.Column<bool>(nullable: false),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    UploaderID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    OriginalFileName = table.Column<string>(maxLength: 256, nullable: true),
                    UploadedTime = table.Column<DateTime>(nullable: false),
                    UploadPath = table.Column<string>(maxLength: 256, nullable: true),
                    UploadFileSuffix = table.Column<string>(maxLength: 256, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    IconString = table.Column<string>(maxLength: 120, nullable: true),
                    IsForTitle = table.Column<bool>(nullable: false),
                    IsUnique = table.Column<bool>(nullable: false),
                    IsAvatar = table.Column<bool>(nullable: false),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    UploaderID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    AttachmentTimeUploaded = table.Column<DateTime>(nullable: false),
                    OriginalFileName = table.Column<string>(maxLength: 500, nullable: true),
                    UploadPath = table.Column<string>(maxLength: 500, nullable: true),
                    IsInDB = table.Column<bool>(nullable: false),
                    UploadFileSuffix = table.Column<string>(maxLength: 10, nullable: true),
                    BinaryContent = table.Column<byte[]>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    IconString = table.Column<string>(maxLength: 120, nullable: true),
                    IsUnique = table.Column<bool>(nullable: false),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    UploaderID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    ProvinceName = table.Column<string>(maxLength: 20, nullable: true),
                    CityName = table.Column<string>(maxLength: 20, nullable: true),
                    CountyName = table.Column<string>(maxLength: 20, nullable: true),
                    DetailName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseContainer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseContainer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseWithRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseWithRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseWithUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseWithUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemoCommons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoCommons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemoEntityParents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoEntityParents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoEntityParents_DemoEntityParents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DemoEntityParents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemoItemForMultiSelects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoItemForMultiSelects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    UndertakerName = table.Column<string>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: false),
                    IsFinished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDemo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicCores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TypeName = table.Column<int>(nullable: false),
                    SingerName = table.Column<string>(nullable: true),
                    PhotoPath = table.Column<string>(nullable: true),
                    MusicPath = table.Column<string>(nullable: true),
                    lyricPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicCores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    MusicTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicType_MusicType_MusicTypeId",
                        column: x => x.MusicTypeId,
                        principalTable: "MusicType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationKPITypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationKPITypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationMenuItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    UrlString = table.Column<string>(maxLength: 300, nullable: true),
                    IconString = table.Column<string>(maxLength: 50, nullable: true),
                    ItemStartTipString = table.Column<string>(maxLength: 50, nullable: true),
                    ItemEndString = table.Column<string>(nullable: true),
                    ParentItemId = table.Column<Guid>(nullable: true),
                    ApplicationMenuGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationMenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationMenuItems_ApplicationMenuGroups_ApplicationMenuGroupId",
                        column: x => x.ApplicationMenuGroupId,
                        principalTable: "ApplicationMenuGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationMenuItems_ApplicationMenuItems_ParentItemId",
                        column: x => x.ParentItemId,
                        principalTable: "ApplicationMenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    ArticleSecondTitle = table.Column<string>(maxLength: 200, nullable: true),
                    ArticleSource = table.Column<string>(maxLength: 250, nullable: true),
                    ArticleContent = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: false),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    IsPassed = table.Column<bool>(nullable: false),
                    IsPublishedByHtml = table.Column<bool>(nullable: false),
                    IsOriented = table.Column<bool>(nullable: false),
                    UpVoteNumber = table.Column<int>(nullable: false),
                    RelevanceObjectID = table.Column<Guid>(nullable: false),
                    SourceType = table.Column<int>(nullable: false),
                    ArticleStatus = table.Column<int>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseItemContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    SecondTitle = table.Column<string>(maxLength: 200, nullable: true),
                    HeadContent = table.Column<string>(maxLength: 500, nullable: true),
                    FootContent = table.Column<string>(maxLength: 500, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    BodyContent = table.Column<string>(nullable: true),
                    EditorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItemContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseItemContents_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    TopicImageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTopics_BusinessImages_TopicImageId",
                        column: x => x.TopicImageId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    ParentTypeId = table.Column<Guid>(nullable: true),
                    ArticleTypeImageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTypes_BusinessImages_ArticleTypeImageId",
                        column: x => x.ArticleTypeImageId,
                        principalTable: "BusinessImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTypes_ArticleTypes_ParentTypeId",
                        column: x => x.ParentTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCenterRegisters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    AdminUserName = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: true),
                    BusinessEntityStatusEnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCenterRegisters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCenterRegisters_CommonAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CommonAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: false),
                    CourseContainerId = table.Column<Guid>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: true),
                    CourseAdministratorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_CourseAdministratorId",
                        column: x => x.CourseAdministratorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_CourseContainer_CourseContainerId",
                        column: x => x.CourseContainerId,
                        principalTable: "CourseContainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemoEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Password = table.Column<string>(maxLength: 20, nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    OrderDateTime = table.Column<DateTime>(nullable: false),
                    IsFinished = table.Column<bool>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    HtmlContent = table.Column<string>(nullable: true),
                    DemoEntityParentId = table.Column<Guid>(nullable: true),
                    DemoEntityEnum = table.Column<int>(nullable: false),
                    DemoCommonId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoEntities_DemoCommons_DemoCommonId",
                        column: x => x.DemoCommonId,
                        principalTable: "DemoCommons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemoEntities_DemoEntityParents_DemoEntityParentId",
                        column: x => x.DemoEntityParentId,
                        principalTable: "DemoEntityParents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Music",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    SingerName = table.Column<string>(nullable: true),
                    MusicTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Music", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Music_MusicType_MusicTypeId",
                        column: x => x.MusicTypeId,
                        principalTable: "MusicType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Comment = table.Column<string>(maxLength: 1000, nullable: true),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    CommentWritorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComments_AspNetUsers_CommentWritorId",
                        column: x => x.CommentWritorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleComments_ArticleComments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ArticleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleRelevances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    RelevanceArticleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleRelevances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleRelevances_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleRelevances_Articles_RelevanceArticleId",
                        column: x => x.RelevanceArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleWithTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: true),
                    TagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWithTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleWithTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWithTags_ArticleTags_TagId",
                        column: x => x.TagId,
                        principalTable: "ArticleTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleInTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: true),
                    SortCode = table.Column<string>(maxLength: 50, nullable: true),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    ArticleTopicId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleInTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleInTopics_ArticleTopics_ArticleTopicId",
                        column: x => x.ArticleTopicId,
                        principalTable: "ArticleTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleInTopics_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleInTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MasterArticleId = table.Column<Guid>(nullable: true),
                    ArticleTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleInTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleInTypes_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleInTypes_Articles_MasterArticleId",
                        column: x => x.MasterArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    CourseId = table.Column<Guid>(nullable: true),
                    CourseItemContentId = table.Column<Guid>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseItems_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseItems_CourseItemContents_CourseItemContentId",
                        column: x => x.CourseItemContentId,
                        principalTable: "CourseItemContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseItems_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseItems_CourseItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CourseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemoEntityItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    DemoEntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoEntityItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoEntityItems_DemoEntities_DemoEntityId",
                        column: x => x.DemoEntityId,
                        principalTable: "DemoEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemoEntityWithDemoItemForMultiSelects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DemoEntityId = table.Column<Guid>(nullable: true),
                    DemoItemForMultiSelectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoEntityWithDemoItemForMultiSelects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoEntityWithDemoItemForMultiSelects_DemoEntities_DemoEntityId",
                        column: x => x.DemoEntityId,
                        principalTable: "DemoEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemoEntityWithDemoItemForMultiSelects_DemoItemForMultiSelects_DemoItemForMultiSelectId",
                        column: x => x.DemoItemForMultiSelectId,
                        principalTable: "DemoItemForMultiSelects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCommentWithTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommentId = table.Column<Guid>(nullable: true),
                    TagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCommentWithTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleCommentWithTags_ArticleComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "ArticleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleCommentWithTags_ArticleCommentTags_TagId",
                        column: x => x.TagId,
                        principalTable: "ArticleCommentTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    EmployeeAmount = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true),
                    ApplicationRoleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_CommonAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CommonAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentKPIMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    TotalPerformenceValue = table.Column<float>(nullable: false),
                    MetricLifeCycle = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentKPIMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentKPIMetrics_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentKPIs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    Benchmark = table.Column<int>(nullable: false),
                    Coefficient = table.Column<float>(nullable: false),
                    KPITypeId = table.Column<Guid>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentKPIs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentKPIs_OrganizationKPITypes_KPITypeId",
                        column: x => x.KPITypeId,
                        principalTable: "OrganizationKPITypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentKPIMetricItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultValue = table.Column<int>(nullable: false),
                    AdjustmentValue = table.Column<int>(nullable: false),
                    PerformanceValue = table.Column<float>(nullable: false),
                    DepartmentKPIId = table.Column<Guid>(nullable: true),
                    DepartmentKPIMetricId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentKPIMetricItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentKPIMetricItems_DepartmentKPIs_DepartmentKPIId",
                        column: x => x.DepartmentKPIId,
                        principalTable: "DepartmentKPIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentKPIMetricItems_DepartmentKPIMetrics_DepartmentKPIMetricId",
                        column: x => x.DepartmentKPIMetricId,
                        principalTable: "DepartmentKPIMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 26, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CredentialsCode = table.Column<string>(maxLength: 26, nullable: true),
                    PersonalCode = table.Column<string>(maxLength: 50, nullable: true),
                    AvatarPath = table.Column<string>(maxLength: 350, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    ExpiredDateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsStudent = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    PositionId = table.Column<Guid>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_CommonAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CommonAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PositionWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    WorkActionUrl = table.Column<string>(maxLength: 300, nullable: true),
                    PositionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionWorks_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentContacts_Employees_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentContacts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentLeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LeaderId = table.Column<Guid>(nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentLeaders_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentLeaders_Employees_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeKPIMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    TotalPerformenceValue = table.Column<float>(nullable: false),
                    MetricLifeCycle = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeKPIMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIMetrics_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    AdminUserName = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: false),
                    BusinessEntityStatusEnum = table.Column<int>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: true),
                    TransactionCenterRegisterId = table.Column<Guid>(nullable: true),
                    OrganizationLeaderId = table.Column<Guid>(nullable: true),
                    OrganzationContactId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_CommonAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CommonAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organizations_Employees_OrganizationLeaderId",
                        column: x => x.OrganizationLeaderId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organizations_Employees_OrganzationContactId",
                        column: x => x.OrganzationContactId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organizations_TransactionCenterRegisters_TransactionCenterRegisterId",
                        column: x => x.TransactionCenterRegisterId,
                        principalTable: "TransactionCenterRegisters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWithPositionWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: true),
                    PositionWorkId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWithPositionWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWithPositionWorks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeWithPositionWorks_PositionWorks_PositionWorkId",
                        column: x => x.PositionWorkId,
                        principalTable: "PositionWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationBusinessProcessActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    OrganizationBusinessProcessId = table.Column<Guid>(nullable: false),
                    PreviousNodeId = table.Column<Guid>(nullable: false),
                    NextNodeId = table.Column<Guid>(nullable: false),
                    PositionWorkId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationBusinessProcessActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationBusinessProcessActivities_PositionWorks_PositionWorkId",
                        column: x => x.PositionWorkId,
                        principalTable: "PositionWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PositionWorkKPIs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    Benchmark = table.Column<int>(nullable: false),
                    Coefficient = table.Column<float>(nullable: false),
                    PositionId = table.Column<Guid>(nullable: true),
                    KPITypeId = table.Column<Guid>(nullable: true),
                    PositionWorkId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionWorkKPIs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionWorkKPIs_OrganizationKPITypes_KPITypeId",
                        column: x => x.KPITypeId,
                        principalTable: "OrganizationKPITypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionWorkKPIs_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionWorkKPIs_PositionWorks_PositionWorkId",
                        column: x => x.PositionWorkId,
                        principalTable: "PositionWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationBusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    SortCode = table.Column<string>(maxLength: 100, nullable: true),
                    IsPseudoDelete = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationBusinessProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationBusinessProcesses_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeKPIMetricItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultValue = table.Column<int>(nullable: false),
                    AdjustmentValue = table.Column<int>(nullable: false),
                    PerformanceValue = table.Column<float>(nullable: false),
                    PositionWorkKPIId = table.Column<Guid>(nullable: true),
                    EmployeeKPIMetricId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeKPIMetricItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIMetricItems_EmployeeKPIMetrics_EmployeeKPIMetricId",
                        column: x => x.EmployeeKPIMetricId,
                        principalTable: "EmployeeKPIMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeKPIMetricItems_PositionWorkKPIs_PositionWorkKPIId",
                        column: x => x.PositionWorkKPIId,
                        principalTable: "PositionWorkKPIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationBusinessProcessWithWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StepNumber = table.Column<string>(maxLength: 20, nullable: true),
                    OrganizationBusinessProcessId = table.Column<Guid>(nullable: true),
                    PositionWorkId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationBusinessProcessWithWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationBusinessProcessWithWorks_OrganizationBusinessProcesses_OrganizationBusinessProcessId",
                        column: x => x.OrganizationBusinessProcessId,
                        principalTable: "OrganizationBusinessProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationBusinessProcessWithWorks_PositionWorks_PositionWorkId",
                        column: x => x.PositionWorkId,
                        principalTable: "PositionWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationMenuItems_ApplicationMenuGroupId",
                table: "ApplicationMenuItems",
                column: "ApplicationMenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationMenuItems_ParentItemId",
                table: "ApplicationMenuItems",
                column: "ParentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_CommentWritorId",
                table: "ArticleComments",
                column: "CommentWritorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_MasterArticleId",
                table: "ArticleComments",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_ParentId",
                table: "ArticleComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCommentWithTags_CommentId",
                table: "ArticleCommentWithTags",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCommentWithTags_TagId",
                table: "ArticleCommentWithTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTopics_ArticleTopicId",
                table: "ArticleInTopics",
                column: "ArticleTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTopics_MasterArticleId",
                table: "ArticleInTopics",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTypes_ArticleTypeId",
                table: "ArticleInTypes",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleInTypes_MasterArticleId",
                table: "ArticleInTypes",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRelevances_MasterArticleId",
                table: "ArticleRelevances",
                column: "MasterArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRelevances_RelevanceArticleId",
                table: "ArticleRelevances",
                column: "RelevanceArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CreatorUserId",
                table: "Articles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTopics_TopicImageId",
                table: "ArticleTopics",
                column: "TopicImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_ArticleTypeImageId",
                table: "ArticleTypes",
                column: "ArticleTypeImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_ParentTypeId",
                table: "ArticleTypes",
                column: "ParentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithTags_ArticleId",
                table: "ArticleWithTags",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWithTags_TagId",
                table: "ArticleWithTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_ParentRoleId",
                table: "AspNetRoles",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItemContents_EditorId",
                table: "CourseItemContents",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CourseId",
                table: "CourseItems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CourseItemContentId",
                table: "CourseItems",
                column: "CourseItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_CreatorId",
                table: "CourseItems",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItems_ParentId",
                table: "CourseItems",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseAdministratorId",
                table: "Courses",
                column: "CourseAdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseContainerId",
                table: "Courses",
                column: "CourseContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatorId",
                table: "Courses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntities_DemoCommonId",
                table: "DemoEntities",
                column: "DemoCommonId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntities_DemoEntityParentId",
                table: "DemoEntities",
                column: "DemoEntityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntityItems_DemoEntityId",
                table: "DemoEntityItems",
                column: "DemoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntityParents_ParentId",
                table: "DemoEntityParents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntityWithDemoItemForMultiSelects_DemoEntityId",
                table: "DemoEntityWithDemoItemForMultiSelects",
                column: "DemoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoEntityWithDemoItemForMultiSelects_DemoItemForMultiSelectId",
                table: "DemoEntityWithDemoItemForMultiSelects",
                column: "DemoItemForMultiSelectId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentContacts_ContactId",
                table: "DepartmentContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentContacts_DepartmentId",
                table: "DepartmentContacts",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentKPIMetricItems_DepartmentKPIId",
                table: "DepartmentKPIMetricItems",
                column: "DepartmentKPIId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentKPIMetricItems_DepartmentKPIMetricId",
                table: "DepartmentKPIMetricItems",
                column: "DepartmentKPIMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentKPIMetrics_DepartmentId",
                table: "DepartmentKPIMetrics",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentKPIs_DepartmentId",
                table: "DepartmentKPIs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentKPIs_KPITypeId",
                table: "DepartmentKPIs",
                column: "KPITypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLeaders_DepartmentId",
                table: "DepartmentLeaders",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLeaders_LeaderId",
                table: "DepartmentLeaders",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_AddressId",
                table: "Departments",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ApplicationRoleId",
                table: "Departments",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId",
                table: "Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIMetricItems_EmployeeKPIMetricId",
                table: "EmployeeKPIMetricItems",
                column: "EmployeeKPIMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIMetricItems_PositionWorkKPIId",
                table: "EmployeeKPIMetricItems",
                column: "PositionWorkKPIId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeKPIMetrics_EmployeeId",
                table: "EmployeeKPIMetrics",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AddressId",
                table: "Employees",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWithPositionWorks_EmployeeId",
                table: "EmployeeWithPositionWorks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWithPositionWorks_PositionWorkId",
                table: "EmployeeWithPositionWorks",
                column: "PositionWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Music_MusicTypeId",
                table: "Music",
                column: "MusicTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicType_MusicTypeId",
                table: "MusicType",
                column: "MusicTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationBusinessProcessActivities_PositionWorkId",
                table: "OrganizationBusinessProcessActivities",
                column: "PositionWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationBusinessProcesses_OrganizationId",
                table: "OrganizationBusinessProcesses",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationBusinessProcessWithWorks_OrganizationBusinessProcessId",
                table: "OrganizationBusinessProcessWithWorks",
                column: "OrganizationBusinessProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationBusinessProcessWithWorks_PositionWorkId",
                table: "OrganizationBusinessProcessWithWorks",
                column: "PositionWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AddressId",
                table: "Organizations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationLeaderId",
                table: "Organizations",
                column: "OrganizationLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganzationContactId",
                table: "Organizations",
                column: "OrganzationContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_TransactionCenterRegisterId",
                table: "Organizations",
                column: "TransactionCenterRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_DepartmentId",
                table: "Positions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionWorkKPIs_KPITypeId",
                table: "PositionWorkKPIs",
                column: "KPITypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionWorkKPIs_PositionId",
                table: "PositionWorkKPIs",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionWorkKPIs_PositionWorkId",
                table: "PositionWorkKPIs",
                column: "PositionWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionWorks_PositionId",
                table: "PositionWorks",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCenterRegisters_AddressId",
                table: "TransactionCenterRegisters",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Organizations_OrganizationId",
                table: "Departments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetRoles_ApplicationRoleId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Employees_OrganizationLeaderId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Employees_OrganzationContactId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "ApplicationMenuItems");

            migrationBuilder.DropTable(
                name: "ArticleCommentWithTags");

            migrationBuilder.DropTable(
                name: "ArticleInTopics");

            migrationBuilder.DropTable(
                name: "ArticleInTypes");

            migrationBuilder.DropTable(
                name: "ArticleRelevances");

            migrationBuilder.DropTable(
                name: "ArticleWithTags");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BusinessFiles");

            migrationBuilder.DropTable(
                name: "BusinessVideos");

            migrationBuilder.DropTable(
                name: "CourseItems");

            migrationBuilder.DropTable(
                name: "CourseWithRoles");

            migrationBuilder.DropTable(
                name: "CourseWithUsers");

            migrationBuilder.DropTable(
                name: "DemoEntityItems");

            migrationBuilder.DropTable(
                name: "DemoEntityWithDemoItemForMultiSelects");

            migrationBuilder.DropTable(
                name: "DepartmentContacts");

            migrationBuilder.DropTable(
                name: "DepartmentKPIMetricItems");

            migrationBuilder.DropTable(
                name: "DepartmentLeaders");

            migrationBuilder.DropTable(
                name: "EmployeeKPIMetricItems");

            migrationBuilder.DropTable(
                name: "EmployeeWithPositionWorks");

            migrationBuilder.DropTable(
                name: "MDemo");

            migrationBuilder.DropTable(
                name: "Music");

            migrationBuilder.DropTable(
                name: "MusicCores");

            migrationBuilder.DropTable(
                name: "OrganizationBusinessProcessActivities");

            migrationBuilder.DropTable(
                name: "OrganizationBusinessProcessWithWorks");

            migrationBuilder.DropTable(
                name: "ApplicationMenuGroups");

            migrationBuilder.DropTable(
                name: "ArticleComments");

            migrationBuilder.DropTable(
                name: "ArticleCommentTags");

            migrationBuilder.DropTable(
                name: "ArticleTopics");

            migrationBuilder.DropTable(
                name: "ArticleTypes");

            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CourseItemContents");

            migrationBuilder.DropTable(
                name: "DemoEntities");

            migrationBuilder.DropTable(
                name: "DemoItemForMultiSelects");

            migrationBuilder.DropTable(
                name: "DepartmentKPIs");

            migrationBuilder.DropTable(
                name: "DepartmentKPIMetrics");

            migrationBuilder.DropTable(
                name: "EmployeeKPIMetrics");

            migrationBuilder.DropTable(
                name: "PositionWorkKPIs");

            migrationBuilder.DropTable(
                name: "MusicType");

            migrationBuilder.DropTable(
                name: "OrganizationBusinessProcesses");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "BusinessImages");

            migrationBuilder.DropTable(
                name: "CourseContainer");

            migrationBuilder.DropTable(
                name: "DemoCommons");

            migrationBuilder.DropTable(
                name: "DemoEntityParents");

            migrationBuilder.DropTable(
                name: "OrganizationKPITypes");

            migrationBuilder.DropTable(
                name: "PositionWorks");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "TransactionCenterRegisters");

            migrationBuilder.DropTable(
                name: "CommonAddresses");
        }
    }
}

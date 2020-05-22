using System;
using dm_backend.EFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dm_backend.Data
{
    public partial class EFDbContext : DbContext
    {
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<AssignDevice> AssignDevice { get; set; }
        public virtual DbSet<BankDetails> BankDetails { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Complaints> Complaints { get; set; }
        public virtual DbSet<ContactNumber> ContactNumber { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DepartmentDesignation> DepartmentDesignation { get; set; }
        public virtual DbSet<Dependent> Dependent { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceBrand> DeviceBrand { get; set; }
        public virtual DbSet<DeviceType> DeviceType { get; set; }
        public virtual DbSet<EducationDetails> EducationDetails { get; set; }
        public virtual DbSet<EducationStreams> EducationStreams { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<NewDeviceRequest> NewDeviceRequest { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Relation> Relation { get; set; }
        public virtual DbSet<RequestDevice> RequestDevice { get; set; }
        public virtual DbSet<RequestHistory> RequestHistory { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleToPermission> RoleToPermission { get; set; }
        public virtual DbSet<Salutation> Salutation { get; set; }
        public virtual DbSet<Specification> Specification { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAuth> UserAuth { get; set; }
        public virtual DbSet<UserToAddress> UserToAddress { get; set; }
        public virtual DbSet<UserToDependent> UserToDependent { get; set; }
        public virtual DbSet<UserToEducation> UserToEducation { get; set; }
        public virtual DbSet<UserToRole> UserToRole { get; set; }

        public virtual DbSet<DeviceModel> DeviceModel {get ;set ;}

        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=127.0.0.1;user id=root;password=root@1234;port=3306;database=device_management_final;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.HasIndex(e => e.CityId)
                    .HasName("address_to_location_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id_address_idx");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("text");

                entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Pin)
                    .IsRequired()
                    .HasColumnName("PIN")
                    .HasMaxLength(6);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("address_to_city");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_id");
            });

            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("address_type");

                entity.HasIndex(e => e.AddressType1)
                    .HasName("address_type_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.AddressTypeId).HasColumnName("address_type_id");

                entity.Property(e => e.AddressType1)
                    .IsRequired()
                    .HasColumnName("address_type")
                    .HasMaxLength(45);
            });


            modelBuilder.Entity<AssignDevice>(entity =>
            {
                entity.ToTable("assign_device");

                entity.HasIndex(e => e.DeviceId)
                    .HasName("device_id_assign_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("employee_id_assign_idx");

                entity.Property(e => e.AssignDeviceId).HasColumnName("assign_device_id");

                entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.ReturnTo).HasColumnName("return_to");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e=>e.AssignedDate).HasColumnName("assign_date");
                entity.Property(e=>e.ReturnDate).HasColumnName("return_date");
                

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.AssignDevice)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("device_id_assign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AssignDevice)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_id_assign");
            });



            modelBuilder.Entity<BankDetails>(entity =>
            {
                entity.ToTable("bank_details");

                entity.HasIndex(e => e.AadharNumber)
                    .HasName("aadhar_number_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.BankAccountNumber)
                    .HasName("bank_account_number_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IfscCode)
                    .HasName("ifsc_code_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.PanNumber)
                    .HasName("pan_number_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.BankDetailsId).HasColumnName("bank_details_id");

                entity.Property(e => e.AadharNumber)
                    .HasColumnName("aadhar_number")
                    .HasMaxLength(224);

                entity.Property(e => e.BankAccountNumber)
                    .IsRequired()
                    .HasColumnName("bank_account_number")
                    .HasMaxLength(224);

                entity.Property(e => e.BankBranchName)
                    .IsRequired()
                    .HasColumnName("bank_branch_name")
                    .HasMaxLength(45);

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasColumnName("bank_name")
                    .HasMaxLength(45);

                entity.Property(e => e.IfscCode)
                    .IsRequired()
                    .HasColumnName("ifsc_code")
                    .HasMaxLength(224);

                entity.Property(e => e.PanNumber)
                    .IsRequired()
                    .HasColumnName("pan_number")
                    .HasMaxLength(224);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.BankDetails)
                    .HasForeignKey<BankDetails>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bank_details_to_user");
            });
            
              modelBuilder.Entity<DeviceModel>(entity =>
            {
                entity.ToTable("device_model");

                entity.Property(e => e.DeviceModelId).HasColumnName("device_model_id");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnName("model")
                    .HasMaxLength(45);
            });


            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasIndex(e => e.CityId)
                    .HasName("city_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.StateId)
                    .HasName("city_to_state_idx");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasColumnName("city_name")
                    .HasMaxLength(45);

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("city_to_state");
            });

           modelBuilder.Entity<Complaints>(entity =>
            {
                entity.HasKey(e => e.ComplaintId);

                entity.ToTable("complaints");

                entity.HasIndex(e => e.ComplaintStatusId)
                    .HasName("complaint_status_id_to_status_idx");

                entity.HasIndex(e => e.DeviceId)
                    .HasName("device_id_complaints_idx");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("employee_id_complaints_idx");

                entity.Property(e => e.ComplaintId).HasColumnName("complaint_id");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasColumnName("comments")
                    .HasMaxLength(50);

                entity.Property(e => e.ComplaintStatusId)
                    .HasColumnName("complaint_status_id")
                    .HasDefaultValueSql("'11'");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("blob");

                entity.Property(e => e.ImagePath)
                    .HasColumnName("image_path")
                    .HasMaxLength(200);

                entity.HasOne(d => d.ComplaintStatus)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.ComplaintStatusId)
                    .HasConstraintName("complaint_status_id");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("device_id_complaints");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_id_complaints");
            });




            modelBuilder.Entity<ContactNumber>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("contact_number");

                entity.HasIndex(e => e.ContactTypeId)
                    .HasName("contact_to_type_idx");

                entity.HasIndex(e => e.CountryId)
                    .HasName("contact_to_country_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id_idx");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.AreaCode)
                    .HasColumnName("area_code")
                    .HasMaxLength(6);

                entity.Property(e => e.ContactTypeId).HasColumnName("contact_type_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasColumnName("number")
                    .HasMaxLength(11);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasDefaultValueSql("'16'");

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.ContactNumber)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contact_to_type");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.ContactNumber)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("country_to_contact");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ContactNumber)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_id_contact");
            });

            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.ToTable("contact_type");

                entity.HasIndex(e => e.ContactType1)
                    .HasName("contact_type_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ContactTypeId)
                    .HasName("contact_type_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ContactTypeId).HasColumnName("contact_type_id");

                entity.Property(e => e.ContactType1)
                    .IsRequired()
                    .HasColumnName("contact_type")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.HasIndex(e => e.CountryName)
                    .HasName("country_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(45);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.HasIndex(e => e.CourseName)
                    .HasName("study_level_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasColumnName("course_name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.HasIndex(e => e.DepartmentName)
                    .HasName("department_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasColumnName("department_name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<DepartmentDesignation>(entity =>
            {
                entity.HasKey(e => new { e.DepartmentId, e.DesignationId });

                entity.ToTable("department_designation");

                entity.HasIndex(e => e.DepartmentDesignationId)
                    .HasName("department_designation_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DesignationId)
                    .HasName("designation_map_department_designation_idx");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.DesignationId).HasColumnName("designation_id");

                entity.Property(e => e.DepartmentDesignationId)
                    .HasColumnName("department_designation_id")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DepartmentDesignation)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("department_map_department_designation");

                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.DepartmentDesignation)
                    .HasForeignKey(d => d.DesignationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("designation_map_department_designation");
            });

            modelBuilder.Entity<Dependent>(entity =>
            {
                entity.ToTable("dependent");

                entity.HasIndex(e => e.ContactId)
                    .HasName("dependant_to_contact_idx");

                entity.HasIndex(e => e.DependentId)
                    .HasName("dependant_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.RelationId)
                    .HasName("dependent_to_relation_idx");

                entity.HasIndex(e => e.SalutationId)
                    .HasName("dependent_to_salutation_idx");

                entity.Property(e => e.DependentId).HasColumnName("dependent_id");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(45);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(45);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middle_name")
                    .HasMaxLength(45);

                entity.Property(e => e.RelationId).HasColumnName("relation_id");

                entity.Property(e => e.SalutationId).HasColumnName("salutation_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Dependent)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dependent_to_contact");

                entity.HasOne(d => d.Relation)
                    .WithMany(p => p.Dependent)
                    .HasForeignKey(d => d.RelationId)
                    .HasConstraintName("dependent_to_relation");

                entity.HasOne(d => d.Salutation)
                    .WithMany(p => p.Dependent)
                    .HasForeignKey(d => d.SalutationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dependent_to_salutation");
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.ToTable("designation");

                entity.HasIndex(e => e.Designation1)
                    .HasName("designation_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.DesignationId).HasColumnName("designation_id");

                entity.Property(e => e.Designation1)
                    .IsRequired()
                    .HasColumnName("designation")
                    .HasMaxLength(30);
            });

          modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("device");

                entity.HasIndex(e => e.DeviceBrandId)
                    .HasName("device_brand_idx");

                entity.HasIndex(e => e.DeviceModelId)
                    .HasName("device_model_id_to_model_idx");

                entity.HasIndex(e => e.DeviceTypeId)
                    .HasName("device_type_idx");

                entity.HasIndex(e => e.SerialNumber)
                    .HasName("serial_number_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.SpecificationId)
                    .HasName("specification_id_idx");

                entity.HasIndex(e => e.StatusId)
                    .HasName("status_id_idx");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasMaxLength(50);

                entity.Property(e => e.DeviceBrandId).HasColumnName("device_brand_id");

                entity.Property(e => e.DeviceModelId).HasColumnName("device_model_id");

                entity.Property(e => e.DeviceTypeId).HasColumnName("device_type_id");

                entity.Property(e => e.EntryDate)
                    .HasColumnName("entry_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnName("price")
                    .HasMaxLength(50);
                entity.Property(e => e.PurchaseDate)
                    .HasColumnName("purchase_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("serial_number")
                    .HasMaxLength(50);

                entity.Property(e => e.SpecificationId).HasColumnName("specification_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.WarrantyYear).HasColumnName("warranty_year");

                entity.HasOne(d => d.DeviceBrand)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.DeviceBrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("device_brand");

                entity.HasOne(d => d.DeviceModel)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.DeviceModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("device_model_id_to_model");

                entity.HasOne(d => d.DeviceType)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.DeviceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("device_type");

                entity.HasOne(d => d.Specification)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.SpecificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("specification_id");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("status_id");
            });

            modelBuilder.Entity<DeviceBrand>(entity =>
            {
                entity.ToTable("device_brand");

                entity.Property(e => e.DeviceBrandId).HasColumnName("device_brand_id");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasColumnName("brand")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.ToTable("device_type");

                entity.Property(e => e.DeviceTypeId).HasColumnName("device_type_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EducationDetails>(entity =>
            {
                entity.ToTable("education_details");

                entity.HasIndex(e => e.CourseId)
                    .HasName("edu_details_to_level_idx");

                entity.HasIndex(e => e.StreamId)
                    .HasName("edu_details_to_field_idx");

                entity.Property(e => e.EducationDetailsId).HasColumnName("education_details_id");

                entity.Property(e => e.BoardUniversity)
                    .HasColumnName("board_university")
                    .HasMaxLength(45);

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.InstitutionName)
                    .IsRequired()
                    .HasColumnName("institution_name")
                    .HasMaxLength(100);

                entity.Property(e => e.PercentageGpa)
                    .HasColumnName("percentage_gpa")
                    .HasColumnType("decimal(10,0)");

                entity.Property(e => e.StreamId).HasColumnName("stream_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.EducationDetails)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("edu_details_to_level");

                entity.HasOne(d => d.Stream)
                    .WithMany(p => p.EducationDetails)
                    .HasForeignKey(d => d.StreamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("edu_details_to_field");
            });

            modelBuilder.Entity<EducationStreams>(entity =>
            {
                entity.ToTable("education_streams");

                entity.HasIndex(e => e.StreamName)
                    .HasName("study_field_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.EducationStreamsId).HasColumnName("education_streams_id");

                entity.Property(e => e.StreamName)
                    .IsRequired()
                    .HasColumnName("stream_name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.HasIndex(e => e.Gender1)
                    .HasName("gender_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.GenderId).HasColumnName("gender_id");

                entity.Property(e => e.Gender1)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<NewDeviceRequest>(entity =>
            {
                entity.ToTable("new_device_request");

                entity.Property(e => e.NewDeviceRequestId).HasColumnName("new_device_request_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notification");

                entity.HasIndex(e => e.DeviceId)
                    .HasName("notification_to_device_idx");

                entity.HasIndex(e => e.StatusId)
                    .HasName("notification_to_status_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("employee_id_notification_idx");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(45);

                entity.Property(e => e.NotificationType)
                    .IsRequired()
                    .HasColumnName("notification_type")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");



                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("notification_to_user");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permission");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasColumnName("permission_name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Relation>(entity =>
            {
                entity.ToTable("relation");

                entity.HasIndex(e => e.RelationId)
                    .HasName("relation_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.RelationName)
                    .HasName("relation_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.RelationId).HasColumnName("relation_id");

                entity.Property(e => e.RelationName)
                    .IsRequired()
                    .HasColumnName("relation_name")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<RequestDevice>(entity =>
            {
                entity.ToTable("request_device");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("employeeId_idx");

                entity.HasIndex(e => e.SpecificationId)
                    .HasName("specificationid_idx");

                entity.Property(e => e.RequestDeviceId).HasColumnName("request_device_id");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(50);

                entity.Property(e => e.DeviceBrand)
                    .IsRequired()
                    .HasColumnName("device_brand")
                    .HasMaxLength(50);

                entity.Property(e => e.DeviceModel)
                    .IsRequired()
                    .HasColumnName("device_model")
                    .HasMaxLength(50);

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasColumnName("device_type")
                    .HasMaxLength(50);

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.NoOfDays).HasColumnName("no_of_days");

                entity.Property(e => e.SpecificationId).HasColumnName("specification_id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RequestDevice)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employeeId");

                entity.HasOne(d => d.Specification)
                    .WithMany(p => p.RequestDevice)
                    .HasForeignKey(d => d.SpecificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("specificationid");
            });

            modelBuilder.Entity<RequestHistory>(entity =>
            {
                entity.ToTable("request_history");

                entity.HasIndex(e => e.UserId)
                    .HasName("employee_id_request_idx");

                entity.HasIndex(e => e.SpecificationId)
                    .HasName("specification_id_request_idx");

                entity.HasIndex(e => e.StatusId)
                    .HasName("status_id_request_idx");

                entity.Property(e => e.RequestHistoryId).HasColumnName("request_history_id");

                entity.Property(e => e.AssignDays).HasColumnName("assign_days");

                entity.Property(e => e.DeviceBrand)
                    .IsRequired()
                    .HasColumnName("device_brand")
                    .HasMaxLength(50);

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasColumnName("device_type")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnName("model")
                    .HasMaxLength(50);
                entity.Property(e=>e.AssignedDate).HasColumnName("assign_date");
                entity.Property(e=>e.ReturnDate).HasColumnName("return_date");
               
                entity.Property(e => e.ReturnTo).HasColumnName("return_to");

                entity.Property(e => e.SpecificationId).HasColumnName("specification_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RequestHistory)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("employee_id_request");

                entity.HasOne(d => d.Specification)
                    .WithMany(p => p.RequestHistory)
                    .HasForeignKey(d => d.SpecificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("specification_id_request");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RequestHistory)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("status_id_request");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasIndex(e => e.RoleName)
                    .HasName("role_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<RoleToPermission>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PermissionId });

                entity.ToTable("role_to_permission");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("permission_to_permission_idx");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RoleToPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("permission_to_permission");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleToPermission)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_to_role");
            });

            modelBuilder.Entity<Salutation>(entity =>
            {
                entity.ToTable("salutation");

                entity.HasIndex(e => e.Salutation1)
                    .HasName("salutation_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.SalutationId).HasColumnName("salutation_id");

                entity.Property(e => e.Salutation1)
                    .IsRequired()
                    .HasColumnName("salutation")
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Specification>(entity =>
            {
                entity.ToTable("specification");

                entity.Property(e => e.SpecificationId).HasColumnName("specification_id");

                entity.Property(e => e.Connectivity)
                    .HasColumnName("connectivity")
                    .HasMaxLength(50);

                entity.Property(e => e.Ram)
                    .HasColumnName("RAM")
                    .HasMaxLength(50);

                entity.Property(e => e.ScreenSize)
                    .HasColumnName("screen_size")
                    .HasMaxLength(50);

                entity.Property(e => e.Storage)
                    .HasColumnName("storage")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state");

                entity.HasIndex(e => e.CountryId)
                    .HasName("state_to_country_idx");

                entity.HasIndex(e => e.StateId)
                    .HasName("state_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasColumnName("state_name")
                    .HasMaxLength(45);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("state_to_country");
            });

           modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasColumnName("status_name")
                    .HasMaxLength(45);
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.DepartmentDesignationId)
                    .HasName("user_to_department_designation_id_idx");

                entity.HasIndex(e => e.GenderId)
                    .HasName("user_to_gender_idx");

                entity.HasIndex(e => e.SalutationId)
                    .HasName("user_to_salutation_idx");

                entity.HasIndex(e => e.Status)
                    .HasName("status_id_user_idx");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.DepartmentDesignationId).HasColumnName("department_designation_id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(70);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(45);

                entity.Property(e => e.GenderId).HasColumnName("gender_id");

                entity.Property(e => e.Hashpassword)
                    .HasColumnName("hashpassword")
                    .HasMaxLength(500);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(45);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middle_name")
                    .HasMaxLength(45);

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasMaxLength(45);

                entity.Property(e => e.Saltpassword)
                    .HasColumnName("saltpassword")
                    .HasMaxLength(500);

                entity.Property(e => e.SalutationId)
                    .HasColumnName("salutation_id")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.DepartmentDesignation)
                    .WithMany(p => p.User)
                    .HasPrincipalKey(p => p.DepartmentDesignationId)
                    .HasForeignKey(d => d.DepartmentDesignationId)
                    .HasConstraintName("user_to_department_designation_id");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("user_to_gender");

                entity.HasOne(d => d.Salutation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.SalutationId)
                    .HasConstraintName("user_to_salutation");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("status_id_user");
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.ToTable("user_auth");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Hashpassword)
                    .HasColumnName("hashpassword")
                    .HasMaxLength(500);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(45);

                entity.Property(e => e.Saltpassword)
                    .HasColumnName("saltpassword")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UserToAddress>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.AddressId });

                entity.ToTable("user_to_address");

                entity.HasIndex(e => e.AddressId)
                    .HasName("address_map_address_idx");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.UserToAddress)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("address_map_address");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToAddress)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_map_address_user");
            });

            modelBuilder.Entity<UserToDependent>(entity =>
            {
                entity.ToTable("user_to_dependent");

                entity.HasIndex(e => e.DependentId)
                    .HasName("dependent_map_user_to_dependent_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_map_user_to_dependent_idx");

                entity.Property(e => e.UserToDependentId).HasColumnName("user_to_dependent_id");

                entity.Property(e => e.DependentId).HasColumnName("dependent_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Dependent)
                    .WithMany(p => p.UserToDependent)
                    .HasForeignKey(d => d.DependentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("dependent_map_user_to_dependent");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToDependent)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_map_user_to_dependent");
            });

            modelBuilder.Entity<UserToEducation>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.EducationDetailsId });

                entity.ToTable("user_to_education");

                entity.HasIndex(e => e.EducationDetailsId)
                    .HasName("education_map_user_to_education_idx");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.EducationDetailsId).HasColumnName("education_details_id");

                entity.HasOne(d => d.EducationDetails)
                    .WithMany(p => p.UserToEducation)
                    .HasForeignKey(d => d.EducationDetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("education_map_user_to_education");
            });

            modelBuilder.Entity<UserToRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("user_to_role");

                entity.HasIndex(e => e.RoleId)
                    .HasName("role_id_idx");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserToRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_id_to_role_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_to_user");
            });
        }
    }
}
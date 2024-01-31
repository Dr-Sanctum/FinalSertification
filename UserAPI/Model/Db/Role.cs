namespace UserAPI.Model.Db
{
    public partial class Role
    {
        public RoleId RoleId { get; set; }
        public string Email { get; set; }
        public virtual List<User> Users { get; set;}
    }
}

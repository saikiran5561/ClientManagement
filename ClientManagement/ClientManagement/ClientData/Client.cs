using System;
using System.Collections.Generic;

namespace ClientManagement.ClientData;

public partial class Client
{
    public int ClientId { get; set; }

    public Guid LicenceKey { get; set; }

    public string ClientName { get; set; } = null!;

    public DateTime LicenceStartDate { get; set; }

    public DateTime LicenceEndDate { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

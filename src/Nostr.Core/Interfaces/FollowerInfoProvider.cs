using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nostr.Core.Interfaces;

public interface IFollowerInfoProvider
{
    Task<IEnumerable<string>> GetFollowerInfoAsync(string publicKey, CancellationToken cancellationToken = default);
}
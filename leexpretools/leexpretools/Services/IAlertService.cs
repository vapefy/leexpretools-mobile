using System.Threading.Tasks;

namespace leexpretools.Services {
    public interface IAlertService {
        Task ShowAlertAsync(string title, string message, string cancel);
    }
}
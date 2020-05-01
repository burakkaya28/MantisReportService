using System;
using System.Configuration;

namespace MantisReportService
{
    internal class P
    {
        private string _newBug = string.Empty;
        private string _completedBug = string.Empty;
        private string _reopenBug = string.Empty;
        private string _totalOpenBug = string.Empty;
        private string _allClosedBug = string.Empty;
        private string _allCancelledBug = string.Empty;
        private string _avgClosedDuration = string.Empty;
        private string _allReopenBug = string.Empty;
        private string _allReopenBugAndStillOpen = string.Empty;
        private readonly string _senderMail = ConfigurationManager.AppSettings["SENDER_MAIL"];
        private readonly string _senderMailPass = ConfigurationManager.AppSettings["SENDER_MAIL_PASSWORD"];
        private readonly string _senderHost = ConfigurationManager.AppSettings["SENDER_HOST"];
        private readonly bool _senderSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SENDER_SSL"]);
        private readonly int _senderPort = Convert.ToInt32(ConfigurationManager.AppSettings["SENDER_PORT"]);

        private static void Main()
        {
            var p = new P();
            var db = new Db();

            try
            {
                
                p._newBug = db.GetValueFromDb("SELECT COUNT(*) FROM mantis_bug_table WHERE from_unixtime(date_submitted, '%Y-%m-%d') >= (CURDATE() - INTERVAL 7 DAY)");
                p._completedBug = db.GetValueFromDb("SELECT COUNT(*) closed_bug FROM mantis_bug_table WHERE STATUS IN (80) AND from_unixtime(last_updated, '%Y-%m-%d') >= (CURDATE() - INTERVAL 7 DAY)");
                p._reopenBug = db.GetValueFromDb("SELECT count(DISTINCT bug_id) reopen_bug FROM mantis_bug_history_table WHERE field_name = 'resolution' AND new_value = 30 AND from_unixtime(date_modified, '%Y-%m-%d') >= (CURDATE() - INTERVAL 7 DAY)");
                p._totalOpenBug = db.GetValueFromDb("SELECT count(id) all_open_bug_count FROM mantis_bug_table WHERE STATUS NOT IN(80, 90)");
                p._allClosedBug = db.GetValueFromDb("SELECT SUM(cls.all_closed_bug_count) all_closed_bug_count FROM ( SELECT count(id) all_closed_bug_count FROM mantis_bug_table WHERE STATUS IN(80) UNION SELECT COUNT(id) all_closed_bug_count FROM mantis_bug_table WHERE STATUS IN(90) AND from_unixtime(last_updated, '%Y-%m-%d') < '2017-10-09' ) cls");
                p._allCancelledBug = db.GetValueFromDb("SELECT COUNT(id) all_cancelled_bug_count FROM mantis_bug_table WHERE STATUS IN(90) AND from_unixtime(last_updated, '%Y-%m-%d')  >= '2017-10-09'");
                p._avgClosedDuration = db.GetValueFromDb("SELECT ROUND( SUM( DATEDIFF( from_unixtime( bh.date_modified, '%Y-%m-%d' ), from_unixtime( bo.date_modified, '%Y-%m-%d' ) ) + 1 ) / ( SELECT count( * ) FROM mantis_bug_table WHERE STATUS IN ( 80, 90 )), 2) OPEN_DURATION FROM mantis_bug_table b JOIN mantis_bug_history_table bh ON b.id = bh.bug_id JOIN mantis_bug_history_table bo ON b.id = bo.bug_id WHERE b.STATUS IN ( 80, 90 ) AND bh.new_value IN (80, 90) AND bo.type = '1'");
                p._allReopenBug = db.GetValueFromDb("SELECT count(DISTINCT bug_id) reopen_bug FROM mantis_bug_history_table WHERE field_name = 'resolution' AND new_value = 30");
                p._allReopenBugAndStillOpen = db.GetValueFromDb("SELECT COUNT(distinct bh.bug_id) reopen_and_open FROM mantis_bug_history_table bh JOIN mantis_bug_table b ON b.id = bh.bug_id WHERE bh.field_name = 'resolution' AND bh.new_value = 30 AND b.status not in (80, 90) ");

                new SendMail().SendAnEmail(true, "<span>Haftalık Mantis raporu aşağıda paylaşılmaktadır.</span> <br><div style='height: 5px;'></div> <br><table border='1' style='width: 1050px; font - family: Arial;'><tbody style='text-align: center;'> <tr><th> Bu Hafta Açılan Bug Sayısı </th><th> Bu Hafta Tamamlanan Bug Sayısı </th><th> Bu Hafta Tekrar Açılan Bug Sayısı </th></tr><tr><td> " + p._newBug + " </td><td> " + p._completedBug + " </td><td> " + p._reopenBug + " </td></tr><tr><th> Toplam Açık Bug Sayısı </th><th> Toplam Kapalı Bug Sayısı </th><th> Toplam İptal Edilen Bug Sayısı </th></tr><tr><td> " + p._totalOpenBug + " </td><td> " + p._allClosedBug + " </td><td> " + p._allCancelledBug + " </td></tr><tr><th> Toplam Tekrar Açılan Bug Sayısı </th><th> Toplam Tekrar Açılan ve Hala Açık Bug Sayısı </th><th> Ortalama Bug Çözme Süresi (Gün) </th></tr><tr><td>" + p._allReopenBug + " </td><td>" + p._allReopenBugAndStillOpen + " </td><td>" + p._avgClosedDuration + " </td></tr></tbody></table>", p._senderMail, p._senderMailPass, p._senderHost, p._senderSsl, p._senderPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
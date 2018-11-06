
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/06/2018 11:49:33
-- Generated from EDMX file: C:\Projects\HealthCareAnalytics\HealthCare\HealthCare\DAL_TCG\TCG_Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TCG_Data];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[TCG_DataModelStoreContainer].[spt_fallback_db]', 'U') IS NOT NULL
    DROP TABLE [TCG_DataModelStoreContainer].[spt_fallback_db];
GO
IF OBJECT_ID(N'[TCG_DataModelStoreContainer].[spt_fallback_dev]', 'U') IS NOT NULL
    DROP TABLE [TCG_DataModelStoreContainer].[spt_fallback_dev];
GO
IF OBJECT_ID(N'[TCG_DataModelStoreContainer].[spt_fallback_usg]', 'U') IS NOT NULL
    DROP TABLE [TCG_DataModelStoreContainer].[spt_fallback_usg];
GO
IF OBJECT_ID(N'[TCG_DataModelStoreContainer].[spt_monitor]', 'U') IS NOT NULL
    DROP TABLE [TCG_DataModelStoreContainer].[spt_monitor];
GO
IF OBJECT_ID(N'[TCG_DataModelStoreContainer].[spt_values]', 'U') IS NOT NULL
    DROP TABLE [TCG_DataModelStoreContainer].[spt_values];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Account_AR_Status'
CREATE TABLE [dbo].[Account_AR_Status] (
    [ARSts_ID] int IDENTITY(1,1) NOT NULL,
    [ARSts_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Account_Bill_Status'
CREATE TABLE [dbo].[Account_Bill_Status] (
    [BillSts_ID] int IDENTITY(1,1) NOT NULL,
    [BillSts_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Account_Case_Details'
CREATE TABLE [dbo].[Account_Case_Details] (
    [ACD_ID] int IDENTITY(1,1) NOT NULL,
    [ACD_HspAccID] varchar(50)  NOT NULL,
    [ACD_Amount] varchar(50)  NOT NULL,
    [ACD_Status] int  NOT NULL,
    [ACD_Owner] varchar(50)  NOT NULL,
    [ACD_Type] varchar(50)  NOT NULL,
    [ACD_SubType] varchar(50)  NOT NULL,
    [ACD_PayerReason] int  NOT NULL,
    [ACD_PrimaryReason] varchar(50)  NOT NULL,
    [ACD_SecondaryReason] varchar(50)  NOT NULL,
    [ACD_PrinDiag] varchar(50)  NOT NULL,
    [ACD_PrinProc] varchar(50)  NOT NULL,
    [ACD_Comments] varchar(max)  NULL,
    [ACD_CreatedBy] varchar(50)  NULL,
    [ACD_CreatedDate] datetime  NOT NULL,
    [ACD_UpdatedBy] varchar(50)  NULL,
    [ACD_Updateddate] datetime  NOT NULL,
    [ACD_UpdatedBy_DB] varchar(50)  NULL,
    [ACD_Completed] varchar(50)  NULL,
    [ACD_Priority] varchar(50)  NULL,
    [ACD_Description] varchar(50)  NULL,
    [ACD_TaskFollowUp] int  NOT NULL,
    [ACD_DueDate] datetime  NULL,
    [ACD_FollowUpDate] datetime  NULL,
    [ACD_DeleteFlag] bit  NULL,
    [ACD_TotalCharges] decimal(18,0)  NOT NULL,
    [ACD_TotalPay] decimal(18,0)  NOT NULL,
    [ACD_TotalAdj] decimal(18,0)  NOT NULL,
    [ACD_AmtDiffNAA] decimal(18,0)  NOT NULL,
    [ACD_AmtDiffPayor] decimal(18,0)  NOT NULL,
    [ACD_ExpAmt] decimal(18,0)  NOT NULL,
    [ACD_BillProvider] varchar(100)  NOT NULL,
    [ACD_Department] varchar(100)  NOT NULL,
    [ACD_HBorPB] varchar(100)  NOT NULL
);
GO

-- Creating table 'Account_Case_Detials_History'
CREATE TABLE [dbo].[Account_Case_Detials_History] (
    [ACDH_ID] int IDENTITY(1,1) NOT NULL,
    [ACDH_ACD_ID] int  NOT NULL,
    [ACDH_HspAccID] varchar(50)  NOT NULL,
    [ACDH_Amount] varchar(50)  NOT NULL,
    [ACDH_Status] int  NOT NULL,
    [ACDH_Owner] varchar(50)  NOT NULL,
    [ACDH_Type] varchar(50)  NOT NULL,
    [ACDH_SubType] varchar(50)  NOT NULL,
    [ACDH_PayerReason] int  NOT NULL,
    [ACDH_PrimaryReason] varchar(50)  NOT NULL,
    [ACDH_SecondaryReason] varchar(50)  NOT NULL,
    [ACDH_PrinDiag] varchar(50)  NOT NULL,
    [ACDH_PrinProc] varchar(50)  NOT NULL,
    [ACDH_Comments] varchar(max)  NULL,
    [ACDH_CreatedBy] varchar(50)  NULL,
    [ACDH_CreatedDate] datetime  NOT NULL,
    [ACDH_UpdatedBy_DB] varchar(50)  NULL,
    [ACDH_Completed] varchar(50)  NULL,
    [ACDH_Priority] varchar(50)  NULL,
    [ACDH_Description] varchar(50)  NULL,
    [ACDH_TaskFollowUp] int  NOT NULL,
    [ACDH_DueDate] datetime  NULL,
    [ACDH_FollowUpDate] datetime  NULL,
    [ACDH_DeleteFlag] bit  NULL,
    [ACDH_TotalCharges] decimal(18,0)  NOT NULL,
    [ACDH_TotalPay] decimal(18,0)  NOT NULL,
    [ACDH_TotalAdj] decimal(18,0)  NOT NULL,
    [ACDH_AmtDiffNAA] decimal(18,0)  NOT NULL,
    [ACDH_AmtDiffPayor] decimal(18,0)  NOT NULL,
    [ACDH_ExpAmt] decimal(18,0)  NOT NULL,
    [ACDH_BillProvider] varchar(100)  NOT NULL,
    [ACDH_Department] varchar(100)  NOT NULL,
    [ACDH_HBorPB] varchar(100)  NOT NULL
);
GO

-- Creating table 'Account_Case_Task'
CREATE TABLE [dbo].[Account_Case_Task] (
    [ACT_ID] int IDENTITY(1,1) NOT NULL,
    [ACT_HspAccID] varchar(50)  NOT NULL,
    [ACT_ACD_ID] int  NOT NULL,
    [ACT_Completed] bit  NOT NULL,
    [ACT_Priority] varchar(50)  NOT NULL,
    [ACT_Description] varchar(50)  NOT NULL,
    [ACT_Owner] varchar(50)  NOT NULL,
    [ACT_Comment] varchar(max)  NULL,
    [ACT_DueDate] datetime  NOT NULL,
    [ACT_CreatedBy] varchar(50)  NULL,
    [ACT_CreatedDate] datetime  NOT NULL,
    [ACT_UpdatedBy] varchar(50)  NULL,
    [ACT_Updateddate] datetime  NOT NULL,
    [ACT_UpdatedBy_DB] varchar(50)  NULL,
    [ACT_DeleteFlag] int  NULL
);
GO

-- Creating table 'Account_Case_Task_History'
CREATE TABLE [dbo].[Account_Case_Task_History] (
    [ACTH_ID] int IDENTITY(1,1) NOT NULL,
    [ACTH_ACT_ID] int  NOT NULL,
    [ACTH_ACD_ID] int  NOT NULL,
    [ACTH_HspAccID] varchar(50)  NOT NULL,
    [ACTH_Completed] bit  NOT NULL,
    [ACTH_Priority] varchar(50)  NOT NULL,
    [ACTH_Description] varchar(50)  NOT NULL,
    [ACTH_Owner] varchar(50)  NOT NULL,
    [ACTH_Comment] varchar(max)  NULL,
    [ACTH_DueDate] datetime  NOT NULL,
    [ACTH_CreatedBy] varchar(50)  NULL,
    [ACTH_CreatedDate] datetime  NOT NULL,
    [ACTH_UpdatedBy_DB] varchar(50)  NULL,
    [ACTH_DeleteFlag] int  NULL
);
GO

-- Creating table 'Account_Source'
CREATE TABLE [dbo].[Account_Source] (
    [AccSrc_ID] int IDENTITY(1,1) NOT NULL,
    [AccSrc_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'CaseCompleted_Master'
CREATE TABLE [dbo].[CaseCompleted_Master] (
    [CC_ID] int IDENTITY(1,1) NOT NULL,
    [CC_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'DenialCat_Master'
CREATE TABLE [dbo].[DenialCat_Master] (
    [DC_ID] int IDENTITY(1,1) NOT NULL,
    [DC_Name] varchar(250)  NOT NULL
);
GO

-- Creating table 'DenialStatus_Master'
CREATE TABLE [dbo].[DenialStatus_Master] (
    [DS_ID] int IDENTITY(1,1) NOT NULL,
    [DS_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'Encounter_Type'
CREATE TABLE [dbo].[Encounter_Type] (
    [EncType_ID] int IDENTITY(1,1) NOT NULL,
    [EncType_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Insurance_Company_Name'
CREATE TABLE [dbo].[Insurance_Company_Name] (
    [InsCmp_ID] int IDENTITY(1,1) NOT NULL,
    [InsCmp_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Payor_Financial_Class'
CREATE TABLE [dbo].[Payor_Financial_Class] (
    [PyrFC_ID] int IDENTITY(1,1) NOT NULL,
    [PyrFC_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'PrimaryReason_Master'
CREATE TABLE [dbo].[PrimaryReason_Master] (
    [PRM_ID] int IDENTITY(1,1) NOT NULL,
    [PRM_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'Priority_Master'
CREATE TABLE [dbo].[Priority_Master] (
    [PM_ID] int IDENTITY(1,1) NOT NULL,
    [PM_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [role_key] uniqueidentifier  NOT NULL,
    [role_code] nvarchar(50)  NOT NULL,
    [role_code_short] nvarchar(50)  NOT NULL,
    [role_designation] nvarchar(50)  NOT NULL,
    [role_createdBy_user] nvarchar(50)  NOT NULL,
    [role_created_date] datetime  NOT NULL,
    [role_delete_flag] tinyint  NOT NULL
);
GO

-- Creating table 'RootCause_Master'
CREATE TABLE [dbo].[RootCause_Master] (
    [RC_ID] int IDENTITY(1,1) NOT NULL,
    [RC_Name] varchar(250)  NOT NULL
);
GO

-- Creating table 'Status_Master'
CREATE TABLE [dbo].[Status_Master] (
    [SM_ID] int IDENTITY(1,1) NOT NULL,
    [SM_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'Task_Master'
CREATE TABLE [dbo].[Task_Master] (
    [TM_ID] int IDENTITY(1,1) NOT NULL,
    [TM_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'Underpayment_Reason'
CREATE TABLE [dbo].[Underpayment_Reason] (
    [Reason_Code] int  NOT NULL,
    [Description] nvarchar(255)  NULL
);
GO

-- Creating table 'Underpayments'
CREATE TABLE [dbo].[Underpayments] (
    [Account] varchar(50)  NOT NULL,
    [Acct_Class] nvarchar(255)  NULL,
    [Acct_Status] nvarchar(255)  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Disch_Date] datetime  NULL,
    [Acct_Bal] float  NULL,
    [Message] nvarchar(255)  NULL,
    [Diagnosis_Codes__Mapped___CMS_ICD_10_CM___ICD_9_CM_] nvarchar(50)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Current_NAA_Posted_To_Bucket_] float  NULL,
    [Expected_Allowed_Amount] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Payor_Specified_] float  NULL,
    [Underpayment_Reason_Code] int  NULL,
    [Brief_Summary] nvarchar(4000)  NULL,
    [DateOfUpdate] datetime  NULL,
    [Biller] nvarchar(255)  NULL,
    [BillingProvider] nvarchar(255)  NULL,
    [CaseStatus] varchar(100)  NULL
);
GO

-- Creating table 'Underpayments_Stage'
CREATE TABLE [dbo].[Underpayments_Stage] (
    [Account] nvarchar(100)  NOT NULL,
    [Acct_Class] nvarchar(255)  NULL,
    [Acct_Status] nvarchar(255)  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Disch_Date] datetime  NULL,
    [Acct_Bal] float  NULL,
    [Message] nvarchar(255)  NULL,
    [Diagnosis_Codes] nvarchar(255)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Allowed_Amount_Difference] float  NULL,
    [Expected_Allowed_Amount] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Payor_Specified] float  NULL,
    [Underpayment_Reason_Code] int  NULL,
    [Brief_Summary] nvarchar(4000)  NULL,
    [DateOfUpdate] datetime  NULL,
    [Biller] nvarchar(255)  NULL,
    [BillingProvider] nvarchar(255)  NULL
);
GO

-- Creating table 'User_Login'
CREATE TABLE [dbo].[User_Login] (
    [user_Id] uniqueidentifier  NOT NULL,
    [user_first_name] nvarchar(50)  NOT NULL,
    [user_last_name] nvarchar(50)  NOT NULL,
    [user_web_login] nvarchar(50)  NOT NULL,
    [user_web_pwd] nvarchar(50)  NULL
);
GO

-- Creating table 'Users_Data'
CREATE TABLE [dbo].[Users_Data] (
    [user_ID] uniqueidentifier  NOT NULL,
    [user_first_name] nvarchar(50)  NOT NULL,
    [user_last_name] nvarchar(50)  NOT NULL,
    [user_full_name] varchar(50)  NULL,
    [user_role_key] uniqueidentifier  NOT NULL,
    [user_phone_number] nvarchar(50)  NULL,
    [user_email_id] nvarchar(50)  NOT NULL,
    [user_added_by] nvarchar(50)  NOT NULL,
    [user_add_date] datetime  NOT NULL,
    [user_updated_by] nvarchar(50)  NULL,
    [user_updated_date] datetime  NULL,
    [user_delete_flag] tinyint  NOT NULL,
    [user_middle_name] nvarchar(50)  NULL,
    [user_web_pwd] nvarchar(50)  NOT NULL,
    [otp_key] nvarchar(50)  NULL,
    [otp_time] datetime  NULL,
    [user_active_flag] tinyint  NOT NULL,
    [user_web_login] nvarchar(50)  NULL
);
GO

-- Creating table 'Denials_Data'
CREATE TABLE [dbo].[Denials_Data] (
    [Invoice] varchar(50)  NOT NULL,
    [Ext_Rsn_Code] nvarchar(50)  NOT NULL,
    [Ln_on_EOB] int  NOT NULL,
    [Rev_Code] int  NULL,
    [CPT_R_Code] nvarchar(50)  NULL,
    [Allowed_Amt] nvarchar(50)  NULL,
    [Denied_Amount] nvarchar(50)  NULL,
    [Billed_Amount] nvarchar(50)  NULL,
    [Bucket_Payor] nvarchar(50)  NOT NULL,
    [Bucket_Plan] nvarchar(150)  NOT NULL,
    [Aging_Bkt] nvarchar(20)  NOT NULL,
    [Owning_Area] nvarchar(50)  NULL,
    [Source_Area] nvarchar(50)  NULL,
    [Status] nvarchar(50)  NOT NULL,
    [Root_Cause] nvarchar(150)  NULL,
    [Preventable] varchar(10)  NULL,
    [Follow_Up_Date] nvarchar(1)  NULL,
    [Resolution_or_Resp_Cat] nvarchar(100)  NULL,
    [Rcvd_Dt] nvarchar(50)  NOT NULL,
    [Acct_Class] nvarchar(50)  NOT NULL,
    [Account_ID] bigint  NOT NULL,
    [Account_Name] nvarchar(50)  NOT NULL,
    [Account_Location] nvarchar(50)  NOT NULL,
    [Follow_up_ID] int  NULL,
    [First_Payor_Rcvd_Dt] nvarchar(50)  NULL,
    [Last_Payor_Rcvd_Dt] nvarchar(50)  NULL,
    [Days_Denied] nvarchar(50)  NULL,
    [Denial_Cat] nvarchar(50)  NULL,
    [Ext_Rsn_Code_w__Desc] nvarchar(500)  NULL,
    [First_Claim_Dt] nvarchar(50)  NULL,
    [HB_Acct_Bal] nvarchar(50)  NULL,
    [Acct_Status] nvarchar(50)  NOT NULL,
    [Paid_Amount] nvarchar(50)  NULL,
    [Last_Reopened_Date] datetime  NULL,
    [Last_User_for_Closed_Rec] nvarchar(50)  NULL,
    [Last_Completed_Date] datetime  NULL,
    [Rsn_Code] nvarchar(50)  NOT NULL,
    [Reporting_Rsn_Code_w_Desc] nvarchar(500)  NOT NULL,
    [Service_Area] nvarchar(50)  NOT NULL,
    [Rev_Code_Desc] nvarchar(500)  NULL,
    [Source] nvarchar(50)  NOT NULL,
    [Source_Dept] nvarchar(50)  NULL,
    [Stage] bigint  NULL,
    [Age] int  NOT NULL,
    [Clinical_Root_Cause] nvarchar(25)  NULL,
    [Financial_Class] nvarchar(50)  NOT NULL,
    [First_Ext_Claim_Sent_Date] datetime  NULL,
    [Src_Pmt_Tx] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Forgot_Pwd'
CREATE TABLE [dbo].[Forgot_Pwd] (
    [user_first_name] nvarchar(50)  NOT NULL,
    [user_email_id] nvarchar(50)  NOT NULL,
    [new_pwd] varbinary(150)  NOT NULL
);
GO

-- Creating table 'Timely_Filing_Limits'
CREATE TABLE [dbo].[Timely_Filing_Limits] (
    [Payer] nvarchar(255)  NOT NULL,
    [FileAsPrimary] nvarchar(255)  NOT NULL,
    [PrimaryInMonths] int  NOT NULL,
    [PrimaryInDays] int  NOT NULL,
    [FileAsSecondary] nvarchar(255)  NOT NULL,
    [SecondaryInMonths] int  NOT NULL,
    [SecondaryInDays] int  NOT NULL,
    [AppealTimeFrame] nvarchar(255)  NULL,
    [AppealInMonths] float  NOT NULL,
    [AppealInDays] float  NOT NULL
);
GO

-- Creating table 'VW_AR_AGE_CATEGORY'
CREATE TABLE [dbo].[VW_AR_AGE_CATEGORY] (
    [Hospital_Account_ID] varchar(50)  NULL,
    [Admission_Date] datetime  NULL,
    [First_Billed_Date] datetime  NULL,
    [Age] varchar(14)  NOT NULL,
    [Amount] decimal(18,2)  NULL,
    [Agency] varchar(50)  NULL,
    [Collectable] int  NULL,
    [Primary_Coverage_Payor_Name] varchar(150)  NULL,
    [Total_Account_Balance] decimal(19,4)  NULL,
    [Primary_Coverage_Payor_Financial_Class] varchar(50)  NULL,
    [Account_AR_Status] varchar(50)  NULL,
    [Total_Primary_Insurance_Balance] decimal(19,4)  NULL,
    [Account_Department_Name] varchar(150)  NULL,
    [Account_Location_Name] varchar(150)  NULL,
    [Account_Service_Area_Name] varchar(150)  NULL,
    [Account_Bill_Status] varchar(50)  NULL,
    [Encounter_Type] varchar(50)  NULL,
    [Account_Source] varchar(50)  NULL
);
GO

-- Creating table 'VW_AR_AGE_CATEGORY_CREDIT'
CREATE TABLE [dbo].[VW_AR_AGE_CATEGORY_CREDIT] (
    [Hospital_Account_ID] varchar(50)  NULL,
    [Admission_Date] datetime  NULL,
    [First_Billed_Date] datetime  NULL,
    [Age] varchar(14)  NOT NULL,
    [Amount] decimal(18,2)  NULL,
    [Agency] varchar(50)  NULL,
    [Collectable] int  NULL,
    [Primary_Coverage_Payor_Name] varchar(150)  NULL,
    [Total_Account_Balance] decimal(19,4)  NULL,
    [Primary_Coverage_Payor_Financial_Class] varchar(50)  NULL,
    [Account_AR_Status] varchar(50)  NULL,
    [Total_Primary_Insurance_Balance] decimal(19,4)  NULL,
    [Account_Department_Name] varchar(150)  NULL,
    [Account_Location_Name] varchar(150)  NULL,
    [Account_Service_Area_Name] varchar(150)  NULL,
    [Account_Bill_Status] varchar(50)  NULL,
    [Encounter_Type] varchar(50)  NULL,
    [Account_Source] varchar(50)  NULL
);
GO

-- Creating table 'VW_AR_AGE_CATEGORY_DEBIT'
CREATE TABLE [dbo].[VW_AR_AGE_CATEGORY_DEBIT] (
    [Hospital_Account_ID] varchar(50)  NULL,
    [Admission_Date] datetime  NULL,
    [First_Billed_Date] datetime  NULL,
    [Age] varchar(14)  NOT NULL,
    [Amount] decimal(18,2)  NULL,
    [Agency] varchar(50)  NULL,
    [Collectable] int  NULL,
    [Primary_Coverage_Payor_Name] varchar(150)  NULL,
    [Total_Account_Balance] decimal(19,4)  NULL,
    [Primary_Coverage_Payor_Financial_Class] varchar(50)  NULL,
    [Account_AR_Status] varchar(50)  NULL,
    [Total_Primary_Insurance_Balance] decimal(19,4)  NULL,
    [Account_Department_Name] varchar(150)  NULL,
    [Account_Location_Name] varchar(150)  NULL,
    [Account_Service_Area_Name] varchar(150)  NULL,
    [Account_Bill_Status] varchar(50)  NULL,
    [Encounter_Type] varchar(50)  NULL,
    [Account_Source] varchar(50)  NULL
);
GO

-- Creating table 'HBorPB_Master'
CREATE TABLE [dbo].[HBorPB_Master] (
    [HP_ID] int IDENTITY(1,1) NOT NULL,
    [HP_Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'Underpayment_ReasonCode'
CREATE TABLE [dbo].[Underpayment_ReasonCode] (
    [Reason_Title] nvarchar(255)  NULL,
    [Reason_Code] int  NOT NULL
);
GO

-- Creating table 'Underpayments_APD'
CREATE TABLE [dbo].[Underpayments_APD] (
    [Account] varchar(50)  NOT NULL,
    [Acct_Class] nvarchar(255)  NULL,
    [Acct_Status] nvarchar(255)  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Disch_Date] datetime  NULL,
    [Acct_Bal] float  NULL,
    [Message] nvarchar(255)  NULL,
    [Diagnosis_Codes__Mapped___CMS_ICD_10_CM___ICD_9_CM_] nvarchar(50)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Current_NAA_Posted_To_Bucket_] float  NULL,
    [Expected_Allowed_Amount] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Payor_Specified_] float  NULL,
    [Underpayment_Reason_Code] int  NULL,
    [Brief_Summary] nvarchar(4000)  NULL,
    [DateOfUpdate] datetime  NULL,
    [Biller] nvarchar(255)  NULL,
    [BillingProvider] nvarchar(255)  NULL,
    [CaseStatus] varchar(100)  NULL,
    [DOB] datetime  NULL,
    [SSN] nvarchar(255)  NULL,
    [Adm_Ser_Date] datetime  NULL,
    [Bill_Date] datetime  NULL,
    [Claim_Date] datetime  NULL,
    [Total_Charges] float  NULL,
    [Financial_Class] nvarchar(255)  NULL,
    [Ins1_ID__] nvarchar(255)  NULL,
    [INS1_Balance] float  NULL,
    [Ins2_ID__] nvarchar(255)  NULL,
    [INS2_Mnemonic] nvarchar(255)  NULL,
    [Ins2_Payer] nvarchar(255)  NULL,
    [INS2_Balance] float  NULL,
    [Ins3_ID] nvarchar(255)  NULL,
    [INS3_Mnemonic] nvarchar(255)  NULL,
    [Ins3_Payer] nvarchar(255)  NULL,
    [INS3_Balance] nvarchar(255)  NULL,
    [Pt_Balance] float  NULL
);
GO

-- Creating table 'Underpayments_PB'
CREATE TABLE [dbo].[Underpayments_PB] (
    [Account] varchar(50)  NOT NULL,
    [Acct_Class] nvarchar(255)  NULL,
    [Acct_Status] nvarchar(255)  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Disch_Date] datetime  NULL,
    [Acct_Bal] float  NULL,
    [Message] nvarchar(255)  NULL,
    [Diagnosis_Codes__Mapped___CMS_ICD_10_CM___ICD_9_CM_] nvarchar(50)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Current_NAA_Posted_To_Bucket_] float  NULL,
    [Expected_Allowed_Amount] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Payor_Specified_] float  NULL,
    [Underpayment_Reason_Code] int  NULL,
    [Brief_Summary] nvarchar(4000)  NULL,
    [DateOfUpdate] datetime  NULL,
    [Biller] nvarchar(255)  NULL,
    [BillingProvider] nvarchar(255)  NULL,
    [TOTALCHARGES] float  NULL,
    [TOTALPAYMENTS] float  NULL,
    [TOTALADJUSTMENTS] float  NULL,
    [FIRSTPAYMENTDATE] datetime  NULL,
    [FIRSTPAYMENTAMT] float  NULL,
    [LASTPAYMENTDATE] datetime  NULL,
    [LASTPAYMENTAMT] float  NULL,
    [CPTCODES] nvarchar(255)  NULL,
    [DEPARTMENTID] float  NULL,
    [DEPARTMENTNAME] nvarchar(255)  NULL
);
GO

-- Creating table 'UnderPayments_Stage_HB'
CREATE TABLE [dbo].[UnderPayments_Stage_HB] (
    [Account] nvarchar(255)  NOT NULL,
    [Acct_Class] nvarchar(255)  NULL,
    [Acct_Status] nvarchar(255)  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Disch_Date] datetime  NULL,
    [Acct_Bal] float  NULL,
    [Message] nvarchar(255)  NULL,
    [Diagnosis_Codes__Mapped___CMS_ICD_10_CM___ICD_9_CM_] nvarchar(255)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Current_NAA_Posted_To_Bucket_] float  NULL,
    [Expected_Allowed_Amount] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Payor_Specified_] float  NULL,
    [Underpayment_Reason_Code] nvarchar(255)  NULL,
    [Brief_Summary] nvarchar(max)  NULL,
    [Date_of_Update] nvarchar(255)  NULL,
    [Biller] nvarchar(255)  NULL,
    [Billing_Provider] nvarchar(255)  NULL
);
GO

-- Creating table 'Underpayment_ReasonCode1'
CREATE TABLE [dbo].[Underpayment_ReasonCode1] (
    [Reason_Code] int  NOT NULL,
    [Reason_Title] nvarchar(255)  NULL,
    [Root_Cause_Category] nvarchar(255)  NULL
);
GO

-- Creating table 'Underpayments_Old'
CREATE TABLE [dbo].[Underpayments_Old] (
    [Account] varchar(50)  NOT NULL,
    [Acct_Class] nvarchar(255)  NULL,
    [Acct_Status] nvarchar(255)  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Disch_Date] datetime  NULL,
    [Acct_Bal] float  NULL,
    [Message] nvarchar(255)  NULL,
    [Diagnosis_Codes__Mapped___CMS_ICD_10_CM___ICD_9_CM_] nvarchar(50)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Current_NAA_Posted_To_Bucket_] float  NULL,
    [Expected_Allowed_Amount] nvarchar(255)  NULL,
    [Allowed_Amount_Difference__Payor_Specified_] float  NULL,
    [Underpayment_Reason_Code] int  NULL,
    [Brief_Summary] nvarchar(4000)  NULL,
    [DateOfUpdate] datetime  NULL,
    [Biller] nvarchar(255)  NULL,
    [BillingProvider] nvarchar(255)  NULL
);
GO

-- Creating table 'UnderPayments_PB_July'
CREATE TABLE [dbo].[UnderPayments_PB_July] (
    [Account] nvarchar(50)  NULL,
    [Acct_Class] varchar(50)  NOT NULL,
    [Acct_Status] int  NULL,
    [Account_Name] nvarchar(255)  NULL,
    [Procedure] nvarchar(255)  NULL,
    [Svc_Dt] datetime  NULL,
    [Ins_Amt] decimal(19,4)  NULL,
    [Billing_Provider] nvarchar(255)  NULL,
    [Payor_Name] nvarchar(255)  NULL,
    [Acct_Bal] decimal(19,4)  NULL,
    [Account_Workqueues] nvarchar(255)  NULL,
    [All_Claim_Payors] nvarchar(255)  NULL,
    [Allow_Discrepancy_Amt] decimal(19,4)  NULL,
    [Plan_Name] nvarchar(255)  NULL,
    [Cur_Plan_Group] nvarchar(255)  NULL,
    [Dr_Cr] nvarchar(255)  NULL,
    [Dept_Specialty] nvarchar(255)  NULL,
    [Diagnosis_Code] nvarchar(max)  NULL,
    [Enc_Specialty] nvarchar(255)  NULL,
    [Prim_Fee_Sched] nvarchar(255)  NULL,
    [Procedure_Code] float  NULL,
    [Procedure_Description] nvarchar(255)  NULL,
    [Underpayment_Reason_Code] int  NULL,
    [Brief_Summary] nvarchar(255)  NULL,
    [DateOfUpdate] datetime  NULL,
    [ServiceDepartmentName] nvarchar(255)  NULL,
    [AccountMostRecentClaimDate] datetime  NULL,
    [TotalChargeAmount] decimal(19,4)  NULL,
    [TotalPaymentAmount] decimal(19,4)  NULL,
    [TotalAdjustmentAmount] decimal(19,4)  NULL,
    [AllowedAmount_ProfessionalOnly] decimal(19,4)  NULL
);
GO

-- Creating table 'spt_fallback_db'
CREATE TABLE [dbo].[spt_fallback_db] (
    [xserver_name] varchar(30)  NOT NULL,
    [xdttm_ins] datetime  NOT NULL,
    [xdttm_last_ins_upd] datetime  NOT NULL,
    [xfallback_dbid] smallint  NULL,
    [name] varchar(30)  NOT NULL,
    [dbid] smallint  NOT NULL,
    [status] smallint  NOT NULL,
    [version] smallint  NOT NULL
);
GO

-- Creating table 'spt_fallback_dev'
CREATE TABLE [dbo].[spt_fallback_dev] (
    [xserver_name] varchar(30)  NOT NULL,
    [xdttm_ins] datetime  NOT NULL,
    [xdttm_last_ins_upd] datetime  NOT NULL,
    [xfallback_low] int  NULL,
    [xfallback_drive] char(2)  NULL,
    [low] int  NOT NULL,
    [high] int  NOT NULL,
    [status] smallint  NOT NULL,
    [name] varchar(30)  NOT NULL,
    [phyname] varchar(127)  NOT NULL
);
GO

-- Creating table 'spt_fallback_usg'
CREATE TABLE [dbo].[spt_fallback_usg] (
    [xserver_name] varchar(30)  NOT NULL,
    [xdttm_ins] datetime  NOT NULL,
    [xdttm_last_ins_upd] datetime  NOT NULL,
    [xfallback_vstart] int  NULL,
    [dbid] smallint  NOT NULL,
    [segmap] int  NOT NULL,
    [lstart] int  NOT NULL,
    [sizepg] int  NOT NULL,
    [vstart] int  NOT NULL
);
GO

-- Creating table 'spt_monitor'
CREATE TABLE [dbo].[spt_monitor] (
    [lastrun] datetime  NOT NULL,
    [cpu_busy] int  NOT NULL,
    [io_busy] int  NOT NULL,
    [idle] int  NOT NULL,
    [pack_received] int  NOT NULL,
    [pack_sent] int  NOT NULL,
    [connections] int  NOT NULL,
    [pack_errors] int  NOT NULL,
    [total_read] int  NOT NULL,
    [total_write] int  NOT NULL,
    [total_errors] int  NOT NULL
);
GO

-- Creating table 'spt_values'
CREATE TABLE [dbo].[spt_values] (
    [name] nvarchar(35)  NULL,
    [number] int  NOT NULL,
    [type] nchar(3)  NOT NULL,
    [low] int  NULL,
    [high] int  NULL,
    [status] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ARSts_ID] in table 'Account_AR_Status'
ALTER TABLE [dbo].[Account_AR_Status]
ADD CONSTRAINT [PK_Account_AR_Status]
    PRIMARY KEY CLUSTERED ([ARSts_ID] ASC);
GO

-- Creating primary key on [BillSts_ID] in table 'Account_Bill_Status'
ALTER TABLE [dbo].[Account_Bill_Status]
ADD CONSTRAINT [PK_Account_Bill_Status]
    PRIMARY KEY CLUSTERED ([BillSts_ID] ASC);
GO

-- Creating primary key on [ACD_ID] in table 'Account_Case_Details'
ALTER TABLE [dbo].[Account_Case_Details]
ADD CONSTRAINT [PK_Account_Case_Details]
    PRIMARY KEY CLUSTERED ([ACD_ID] ASC);
GO

-- Creating primary key on [ACDH_ID] in table 'Account_Case_Detials_History'
ALTER TABLE [dbo].[Account_Case_Detials_History]
ADD CONSTRAINT [PK_Account_Case_Detials_History]
    PRIMARY KEY CLUSTERED ([ACDH_ID] ASC);
GO

-- Creating primary key on [ACT_ID] in table 'Account_Case_Task'
ALTER TABLE [dbo].[Account_Case_Task]
ADD CONSTRAINT [PK_Account_Case_Task]
    PRIMARY KEY CLUSTERED ([ACT_ID] ASC);
GO

-- Creating primary key on [ACTH_ID] in table 'Account_Case_Task_History'
ALTER TABLE [dbo].[Account_Case_Task_History]
ADD CONSTRAINT [PK_Account_Case_Task_History]
    PRIMARY KEY CLUSTERED ([ACTH_ID] ASC);
GO

-- Creating primary key on [AccSrc_ID] in table 'Account_Source'
ALTER TABLE [dbo].[Account_Source]
ADD CONSTRAINT [PK_Account_Source]
    PRIMARY KEY CLUSTERED ([AccSrc_ID] ASC);
GO

-- Creating primary key on [CC_ID] in table 'CaseCompleted_Master'
ALTER TABLE [dbo].[CaseCompleted_Master]
ADD CONSTRAINT [PK_CaseCompleted_Master]
    PRIMARY KEY CLUSTERED ([CC_ID] ASC);
GO

-- Creating primary key on [DC_ID] in table 'DenialCat_Master'
ALTER TABLE [dbo].[DenialCat_Master]
ADD CONSTRAINT [PK_DenialCat_Master]
    PRIMARY KEY CLUSTERED ([DC_ID] ASC);
GO

-- Creating primary key on [DS_ID] in table 'DenialStatus_Master'
ALTER TABLE [dbo].[DenialStatus_Master]
ADD CONSTRAINT [PK_DenialStatus_Master]
    PRIMARY KEY CLUSTERED ([DS_ID] ASC);
GO

-- Creating primary key on [EncType_ID] in table 'Encounter_Type'
ALTER TABLE [dbo].[Encounter_Type]
ADD CONSTRAINT [PK_Encounter_Type]
    PRIMARY KEY CLUSTERED ([EncType_ID] ASC);
GO

-- Creating primary key on [InsCmp_ID] in table 'Insurance_Company_Name'
ALTER TABLE [dbo].[Insurance_Company_Name]
ADD CONSTRAINT [PK_Insurance_Company_Name]
    PRIMARY KEY CLUSTERED ([InsCmp_ID] ASC);
GO

-- Creating primary key on [PyrFC_ID] in table 'Payor_Financial_Class'
ALTER TABLE [dbo].[Payor_Financial_Class]
ADD CONSTRAINT [PK_Payor_Financial_Class]
    PRIMARY KEY CLUSTERED ([PyrFC_ID] ASC);
GO

-- Creating primary key on [PRM_ID] in table 'PrimaryReason_Master'
ALTER TABLE [dbo].[PrimaryReason_Master]
ADD CONSTRAINT [PK_PrimaryReason_Master]
    PRIMARY KEY CLUSTERED ([PRM_ID] ASC);
GO

-- Creating primary key on [PM_ID] in table 'Priority_Master'
ALTER TABLE [dbo].[Priority_Master]
ADD CONSTRAINT [PK_Priority_Master]
    PRIMARY KEY CLUSTERED ([PM_ID] ASC);
GO

-- Creating primary key on [role_key] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([role_key] ASC);
GO

-- Creating primary key on [RC_ID] in table 'RootCause_Master'
ALTER TABLE [dbo].[RootCause_Master]
ADD CONSTRAINT [PK_RootCause_Master]
    PRIMARY KEY CLUSTERED ([RC_ID] ASC);
GO

-- Creating primary key on [SM_ID] in table 'Status_Master'
ALTER TABLE [dbo].[Status_Master]
ADD CONSTRAINT [PK_Status_Master]
    PRIMARY KEY CLUSTERED ([SM_ID] ASC);
GO

-- Creating primary key on [TM_ID] in table 'Task_Master'
ALTER TABLE [dbo].[Task_Master]
ADD CONSTRAINT [PK_Task_Master]
    PRIMARY KEY CLUSTERED ([TM_ID] ASC);
GO

-- Creating primary key on [Reason_Code] in table 'Underpayment_Reason'
ALTER TABLE [dbo].[Underpayment_Reason]
ADD CONSTRAINT [PK_Underpayment_Reason]
    PRIMARY KEY CLUSTERED ([Reason_Code] ASC);
GO

-- Creating primary key on [Account] in table 'Underpayments'
ALTER TABLE [dbo].[Underpayments]
ADD CONSTRAINT [PK_Underpayments]
    PRIMARY KEY CLUSTERED ([Account] ASC);
GO

-- Creating primary key on [Account] in table 'Underpayments_Stage'
ALTER TABLE [dbo].[Underpayments_Stage]
ADD CONSTRAINT [PK_Underpayments_Stage]
    PRIMARY KEY CLUSTERED ([Account] ASC);
GO

-- Creating primary key on [user_Id] in table 'User_Login'
ALTER TABLE [dbo].[User_Login]
ADD CONSTRAINT [PK_User_Login]
    PRIMARY KEY CLUSTERED ([user_Id] ASC);
GO

-- Creating primary key on [user_ID] in table 'Users_Data'
ALTER TABLE [dbo].[Users_Data]
ADD CONSTRAINT [PK_Users_Data]
    PRIMARY KEY CLUSTERED ([user_ID] ASC);
GO

-- Creating primary key on [Invoice], [Ext_Rsn_Code], [Ln_on_EOB], [Bucket_Payor], [Bucket_Plan], [Aging_Bkt], [Status], [Rcvd_Dt], [Acct_Class], [Account_ID], [Account_Name], [Account_Location], [Acct_Status], [Rsn_Code], [Reporting_Rsn_Code_w_Desc], [Service_Area], [Source], [Age], [Financial_Class], [Src_Pmt_Tx] in table 'Denials_Data'
ALTER TABLE [dbo].[Denials_Data]
ADD CONSTRAINT [PK_Denials_Data]
    PRIMARY KEY CLUSTERED ([Invoice], [Ext_Rsn_Code], [Ln_on_EOB], [Bucket_Payor], [Bucket_Plan], [Aging_Bkt], [Status], [Rcvd_Dt], [Acct_Class], [Account_ID], [Account_Name], [Account_Location], [Acct_Status], [Rsn_Code], [Reporting_Rsn_Code_w_Desc], [Service_Area], [Source], [Age], [Financial_Class], [Src_Pmt_Tx] ASC);
GO

-- Creating primary key on [user_first_name], [user_email_id], [new_pwd] in table 'Forgot_Pwd'
ALTER TABLE [dbo].[Forgot_Pwd]
ADD CONSTRAINT [PK_Forgot_Pwd]
    PRIMARY KEY CLUSTERED ([user_first_name], [user_email_id], [new_pwd] ASC);
GO

-- Creating primary key on [Payer], [FileAsPrimary], [PrimaryInMonths], [PrimaryInDays], [FileAsSecondary], [SecondaryInMonths], [SecondaryInDays], [AppealInMonths], [AppealInDays] in table 'Timely_Filing_Limits'
ALTER TABLE [dbo].[Timely_Filing_Limits]
ADD CONSTRAINT [PK_Timely_Filing_Limits]
    PRIMARY KEY CLUSTERED ([Payer], [FileAsPrimary], [PrimaryInMonths], [PrimaryInDays], [FileAsSecondary], [SecondaryInMonths], [SecondaryInDays], [AppealInMonths], [AppealInDays] ASC);
GO

-- Creating primary key on [Age] in table 'VW_AR_AGE_CATEGORY'
ALTER TABLE [dbo].[VW_AR_AGE_CATEGORY]
ADD CONSTRAINT [PK_VW_AR_AGE_CATEGORY]
    PRIMARY KEY CLUSTERED ([Age] ASC);
GO

-- Creating primary key on [Age] in table 'VW_AR_AGE_CATEGORY_CREDIT'
ALTER TABLE [dbo].[VW_AR_AGE_CATEGORY_CREDIT]
ADD CONSTRAINT [PK_VW_AR_AGE_CATEGORY_CREDIT]
    PRIMARY KEY CLUSTERED ([Age] ASC);
GO

-- Creating primary key on [Age] in table 'VW_AR_AGE_CATEGORY_DEBIT'
ALTER TABLE [dbo].[VW_AR_AGE_CATEGORY_DEBIT]
ADD CONSTRAINT [PK_VW_AR_AGE_CATEGORY_DEBIT]
    PRIMARY KEY CLUSTERED ([Age] ASC);
GO

-- Creating primary key on [HP_ID] in table 'HBorPB_Master'
ALTER TABLE [dbo].[HBorPB_Master]
ADD CONSTRAINT [PK_HBorPB_Master]
    PRIMARY KEY CLUSTERED ([HP_ID] ASC);
GO

-- Creating primary key on [Reason_Code] in table 'Underpayment_ReasonCode'
ALTER TABLE [dbo].[Underpayment_ReasonCode]
ADD CONSTRAINT [PK_Underpayment_ReasonCode]
    PRIMARY KEY CLUSTERED ([Reason_Code] ASC);
GO

-- Creating primary key on [Account] in table 'Underpayments_APD'
ALTER TABLE [dbo].[Underpayments_APD]
ADD CONSTRAINT [PK_Underpayments_APD]
    PRIMARY KEY CLUSTERED ([Account] ASC);
GO

-- Creating primary key on [Account] in table 'Underpayments_PB'
ALTER TABLE [dbo].[Underpayments_PB]
ADD CONSTRAINT [PK_Underpayments_PB]
    PRIMARY KEY CLUSTERED ([Account] ASC);
GO

-- Creating primary key on [Account] in table 'UnderPayments_Stage_HB'
ALTER TABLE [dbo].[UnderPayments_Stage_HB]
ADD CONSTRAINT [PK_UnderPayments_Stage_HB]
    PRIMARY KEY CLUSTERED ([Account] ASC);
GO

-- Creating primary key on [Reason_Code] in table 'Underpayment_ReasonCode1'
ALTER TABLE [dbo].[Underpayment_ReasonCode1]
ADD CONSTRAINT [PK_Underpayment_ReasonCode1]
    PRIMARY KEY CLUSTERED ([Reason_Code] ASC);
GO

-- Creating primary key on [Account] in table 'Underpayments_Old'
ALTER TABLE [dbo].[Underpayments_Old]
ADD CONSTRAINT [PK_Underpayments_Old]
    PRIMARY KEY CLUSTERED ([Account] ASC);
GO

-- Creating primary key on [Acct_Class] in table 'UnderPayments_PB_July'
ALTER TABLE [dbo].[UnderPayments_PB_July]
ADD CONSTRAINT [PK_UnderPayments_PB_July]
    PRIMARY KEY CLUSTERED ([Acct_Class] ASC);
GO

-- Creating primary key on [xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [name], [dbid], [status], [version] in table 'spt_fallback_db'
ALTER TABLE [dbo].[spt_fallback_db]
ADD CONSTRAINT [PK_spt_fallback_db]
    PRIMARY KEY CLUSTERED ([xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [name], [dbid], [status], [version] ASC);
GO

-- Creating primary key on [xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [low], [high], [status], [name], [phyname] in table 'spt_fallback_dev'
ALTER TABLE [dbo].[spt_fallback_dev]
ADD CONSTRAINT [PK_spt_fallback_dev]
    PRIMARY KEY CLUSTERED ([xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [low], [high], [status], [name], [phyname] ASC);
GO

-- Creating primary key on [xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [dbid], [segmap], [lstart], [sizepg], [vstart] in table 'spt_fallback_usg'
ALTER TABLE [dbo].[spt_fallback_usg]
ADD CONSTRAINT [PK_spt_fallback_usg]
    PRIMARY KEY CLUSTERED ([xserver_name], [xdttm_ins], [xdttm_last_ins_upd], [dbid], [segmap], [lstart], [sizepg], [vstart] ASC);
GO

-- Creating primary key on [lastrun], [cpu_busy], [io_busy], [idle], [pack_received], [pack_sent], [connections], [pack_errors], [total_read], [total_write], [total_errors] in table 'spt_monitor'
ALTER TABLE [dbo].[spt_monitor]
ADD CONSTRAINT [PK_spt_monitor]
    PRIMARY KEY CLUSTERED ([lastrun], [cpu_busy], [io_busy], [idle], [pack_received], [pack_sent], [connections], [pack_errors], [total_read], [total_write], [total_errors] ASC);
GO

-- Creating primary key on [number], [type] in table 'spt_values'
ALTER TABLE [dbo].[spt_values]
ADD CONSTRAINT [PK_spt_values]
    PRIMARY KEY CLUSTERED ([number], [type] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ACDH_ACD_ID] in table 'Account_Case_Detials_History'
ALTER TABLE [dbo].[Account_Case_Detials_History]
ADD CONSTRAINT [FK__Account_C__ACDH___40F9A68C]
    FOREIGN KEY ([ACDH_ACD_ID])
    REFERENCES [dbo].[Account_Case_Details]
        ([ACD_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Account_C__ACDH___40F9A68C'
CREATE INDEX [IX_FK__Account_C__ACDH___40F9A68C]
ON [dbo].[Account_Case_Detials_History]
    ([ACDH_ACD_ID]);
GO

-- Creating foreign key on [ACT_ACD_ID] in table 'Account_Case_Task'
ALTER TABLE [dbo].[Account_Case_Task]
ADD CONSTRAINT [FK__Account_C__ACT_A__44CA3770]
    FOREIGN KEY ([ACT_ACD_ID])
    REFERENCES [dbo].[Account_Case_Details]
        ([ACD_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Account_C__ACT_A__44CA3770'
CREATE INDEX [IX_FK__Account_C__ACT_A__44CA3770]
ON [dbo].[Account_Case_Task]
    ([ACT_ACD_ID]);
GO

-- Creating foreign key on [ACTH_ACD_ID] in table 'Account_Case_Task_History'
ALTER TABLE [dbo].[Account_Case_Task_History]
ADD CONSTRAINT [FK__Account_C__ACTH___498EEC8D]
    FOREIGN KEY ([ACTH_ACD_ID])
    REFERENCES [dbo].[Account_Case_Details]
        ([ACD_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Account_C__ACTH___498EEC8D'
CREATE INDEX [IX_FK__Account_C__ACTH___498EEC8D]
ON [dbo].[Account_Case_Task_History]
    ([ACTH_ACD_ID]);
GO

-- Creating foreign key on [ACTH_ACT_ID] in table 'Account_Case_Task_History'
ALTER TABLE [dbo].[Account_Case_Task_History]
ADD CONSTRAINT [FK__Account_C__ACTH___489AC854]
    FOREIGN KEY ([ACTH_ACT_ID])
    REFERENCES [dbo].[Account_Case_Task]
        ([ACT_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Account_C__ACTH___489AC854'
CREATE INDEX [IX_FK__Account_C__ACTH___489AC854]
ON [dbo].[Account_Case_Task_History]
    ([ACTH_ACT_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
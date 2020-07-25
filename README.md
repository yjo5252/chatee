# :speech_balloon: Healthee Chatbot Service :runner:

## Topic: Hometraining을 도와줄 Azure based chatbot 
홈 트레이닝 챗봇 **Healthee**는 홈 트레이닝을 하려는 분들에게

**운동을 추천**해주고 **운동 기구, 음식에 대한 정보를 제공**하여 운동을 도와드리는 것을 목표하고 있습니다. 

## TEAM: Chatee
<img src="https://user-images.githubusercontent.com/41981471/86508101-52734500-be18-11ea-90e0-92df415e79d2.JPG" width="50%">

## 과제
[가이드 페이지](https://blog.naver.com/formktmkt/221994807603)

### 1. Gate 1

[Gate1 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate1/Gate1.md)

### 2. Gate 2

[Gate2 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate2/Gate2.md)


### 3. Gate 3 

[Gate 3 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/Gate3/Gate3.md)

### 4. 최종 게이트(Gate 4)

:tada:[Gate 4 제출물, 결과 설명](https://github.com/yjo5252/chatee/blob/master/%EC%B5%9C%EC%A2%85%20%EA%B2%8C%EC%9D%B4%ED%8A%B8(Gate%204)/%EC%B5%9C%EC%A2%85%EA%B2%8C%EC%9D%B4%ED%8A%B8%20%EC%84%A4%EB%AA%85.md):tada:

## Healthee가 제공하는 기능

|||
|---------|----|
|<img src="https://user-images.githubusercontent.com/41438361/88453651-d9f62600-cea3-11ea-9df4-3661cd686d8f.JPG", width=75%>|<img src="https://user-images.githubusercontent.com/41438361/88453716-73bdd300-cea4-11ea-8e1f-8b475738f534.JPG", width=75%>|

## 구조(Final Ver)
* Bots 폴더
  * DialogAndWelcomeBot.cs : 새로운 사용자가 들어왔을 때 인사하는 부분. Echobot을 상속받았다.
  * EchoBot.cs : 기존의 Echobot 변형. 사용자의 입력을 처리한다.

* Dialogs 폴더
  * MainDialog.cs : 가장 상위 Dialog. TutorialDialog를 실행시킨다.
  * CheckUserDialog.cs : 맨 처음에 사용자가 입장했을 때 사용자가 이전에 Healthee를 이용했는지 안했는지 물어보는 Dialog. 만약 예전에 사용해봤으면 사용자의 데이터를 DB에서 가져오고 아니면 튜토리얼을 진행하여 사용자의 정보를 받는다.
  * TutorialDialog.cs : 튜토리얼을 진행하는 Dialog. Waterfall Dialog를 이용하여 질문과 답변이 순차적으로 이루어질 수 있게 구현하였다.
  * ShowFuctionsDialog.cs : 사용자에게 이용가능한 기능을 카드로 보여주는 Dialog.
  * RecommendExerciseDialog.cs : 사용자에게 원하는 부위와 목표에 도움이 되는 운동을 추천해주는 Dialog.
  * RecordDialog.cs : 사용자가 한 운동을 기록할 수 있게 하는 Dialog.
  * RecommendFood.cs : 사용자에게 랜덤으로 음식을 추천해주는 Dialog.
  * RecommendEquipment.cs : 사용자에게 운동 기구를 추천해주는 Dialog.
  * SeeMyCharacterDialog.cs : 사용자가 자신의 캐릭터 상태를 확인할 수 있는 Dialog.
  * SeeMyRecord.cs : 사용자가 자신의 운동 기록을 볼 수 있는 Dialog.

* Resources 폴더
  * CharacterName.json : `TutorialDialog` 에서 캐릭터 이름 설정하라고 하는 Adaptive Card 구현
  * CharacterShow.json : `TutorialDialog` 에서 캐릭터 보여주는 Adaptive Card 구현
  * EquipmentInitial.json : `RecommendEquipmentDialog` 에서 처음 기능 이미지 보여주는 Adaptive Card 구현
  * FoodInitial.json : `RecommendFoodDialog` 에서 처음 기능 이미지 보여주는 Adaptive Card 구현
  * RecordInitial.json : `RecordDialog` 에서 처음 기능 이미지 보여주는 Adaptive Card 구현
  * REFirst.json : `RecommendExercise` 에서 맞춤 추천할건지 물어보는 Adaptive Card 구현
  * REInitial.json : `RecommendExercise` 에서 처음 기능 이미지 보여주는Adaptive Card 구현
  * StartTutorial.json : `TutorialDialog` 에서 처음에 사용자 환영하는 Adaptive Card 구현
  * Welcoming.json : 맨 처음 `DialogAndWelcomBot` 에서 사용자 환영하는 Adaptive Card 구현

* UserProfile.cs : 사용자의 정보를 담을 클래스(사실 이 Healthee에서 필수적인 부분은 아니지만, `TutorialDialog`에서 좀 더 편하게 변수에 접근하기 위해 남겨두었습니다. UserProfile을 쓰는 부분을 UserInfoManager로 바꿔도 무방합니다.)
* UserInfoManager.cs : 사용자의 정보를 관리하기 위한 클래스
* Startup.cs : DialogAndWelcomeBot를 MainDialog로 실행시킨다.
* ModeManager.cs : 봇의 모드(기능 실행 상태)를 관리하기 위한 매니저
* Cards.cs : json형식의 파일을 Adaptive Card로 보여주기 위해 사용되는 매니저
* appsettings.json : 봇의 연결 설정(QnA Maker, MicrosoftApp)


## 코드 설명

### 1. Bots 

실질적인 'Bot'의 역할을 하는 부분. 사용자가 처음 Healthee에 입장했을 때 환영 인사를 하거나, 사용자가 보낸 메세지에 응답을 하기 위한 기능을 구현하는 부분이 존재한다.

#### 1.1 `Bots/DialogAndWelcomeBot.cs`

*사용자가 Healthee 봇에 처음 입장했을 때 환영 인사를 보낸다.*

![1](https://user-images.githubusercontent.com/41438361/87849805-76f61380-c926-11ea-9390-325600857f96.JPG)

```C#
public class DialogAndWelcomeBot<T> : EchoBot<T> where T : Dialog
```

DialogAndWelcomeBot은 EchoBot을 상속받았습니다. EchoBot에 대한 설명은 바로 뒤에서 나옵니다.

**사용자 환영하기**

사용자를 환영 하는 방법은 **`OnMemebersAddedAsync`** method를 활용하는 것입니다. 이 method는 봇이 사용자를 처음 인식했을 때, 즉 사용자가 봇에 처음 입장했을 때 실행되는 함수입니다. 말 그대로 사용자가 추가되었을 때 봇이 할 수 있는 동작을 써 놓는 곳입니다.

![41](https://user-images.githubusercontent.com/41438361/88453991-ff386380-cea6-11ea-98fb-79c5791c0740.JPG)

`OnMembersAddedAsync` 함수안에 위의 코드를 넣었습니다.

![1](https://user-images.githubusercontent.com/41438361/88454160-85a17500-cea8-11ea-87f2-6770f005b105.JPG)

처음에 Healthee는 사용자가 이전에 Healthee를 방문했는지 안했는지 확인하기 위해 사용자 확인 작업을 해야 합니다. 따라서 `ModeManager.mode`와 `MainDialog.is_running_dialog`를 설정합니다. 이것들에 대한 자세한 설명은 뒷 부분에 나옵니다.

![2](https://user-images.githubusercontent.com/41438361/88454179-ac5fab80-cea8-11ea-842b-c9c0e9368c35.JPG)

뒤로 카드를 붙일 `attachments`를 생성하고 `reply.Attachments.Add(객체)`로 객체를 붙여줍니다. 위의 코드에서는 `Cards.CreateAdaptiveCardAttachment("Welcoming.json")`과 'heroCard' 를 이용하여 객체를 생성하여 `attachment`에 붙였습니다. `CreateAdaptiveCardAttachment` method는 위의 코드에서 `Welcoming.json` 파일을 Adaptive Card로 변환하여 사용자에게 보여질 수 있게 했습니다. 이 함수는 뒷부분(Cards)에 설명되어 있습니다.

추가로 `turnContext.SendActivityAsync` method를 이용해서 사용자에게 메세지를 보냅니다.

#### 1.2 `Bots/EchoBot.cs`

*사용자가 봇에 메세지를 보내면 응답을 처리하는 부분*

**사용자 응답에 대응하기**

사용자 응답에 대응하는 방법은 **`OnMessageActivityAsync`** method를 활용하는 것입니다. 사용자 응답에 대응이라 하긴 했지만, 이 함수는 사용자가 봇에 메세지를 보낼때마다 실행이 되는 함수입니다. 즉, 사용자가 봇에 메세지를 보낸 것이 인식이 되면 실행이 됩니다. 

**`OnMessageActivityAsync`** method 안의 내용을 보면 다음과 같습니다.

![2](https://user-images.githubusercontent.com/41438361/88438311-66bcc780-ce43-11ea-9bb1-e36165d163ab.JPG)

먼저 QnaMaker를 불러옵니다. QnA Maker는 말 그대로 질문에 답변을 해줄 수 있게 하는 API로, 사용자가 질문을 하면 올바른 답을 출력할 수 있게 도와줍니다. QnAMaker를 생성하기 위해 `_configuration[값]`을 통해서 `appsettings.json` 파일에 있는 값들에 접근합니다. 이 `appsettings.json`에서 연결을 세팅하는 방법도 뒤에서 보겠습니다.

![3](https://user-images.githubusercontent.com/41438361/88438359-8227d280-ce43-11ea-953a-ee1cfcdfb473.JPG)

사용자의 입력을 `msg_from_user` 변수로 받고, 공백을 제거하여 처리를 했습니다. 

사용자의 입력을 처리한 후에는 **`MainDialog.is_running_dialog`** 가 0인지 아닌지 판별하여 봇을 실행시킵니다.

`MainDialog.is_running_dialog`는 현재 실행중인 Dialog(기능)가 있는지 없는지 확인할 수 있게 하는 변수입니다. `0`이면 현재 실행중인 Dialog가 없다는 뜻입니다.(현재 실행중인 기능이 없다. 기능 실행 명령을 대기중이다.) 이때는 사용자의 입력을 받아 사용자의 입력 메세지에 특정 기능을 나타내는 **키워드**가 있을 경우 해당 기능을 실행시킬 수 있도록 모드)(`ModeManager.mode`)를 설정합니다. 이 모드에 대한 설명도 뒤에서 보겠습니다.

![5](https://user-images.githubusercontent.com/41438361/88438660-34f83080-ce44-11ea-9eda-2a12b6e9906a.JPG)

특히, 사용자 입력에 "목적" 이나 "안녕" 이라는 단어가 포함되면 QnA Maker가 실행될 수 있도록 해놨습니다.

**`MainDialog.is_running_dialog`가 1이면** 현재 실행중인 Dialog가 있다는 말입니다.(현재 실행중인 기능이 있다. 기능이 진행중이다.) 따라서 이때는 현재 실행중인 기능이 있으므로 기능 수행 도중에 다른 모드로 전환이 되어버리면 안됩니다. 따라서 이때는 모드 전환 부분이 없습니다.

![4](https://user-images.githubusercontent.com/41438361/88438548-f06c9500-ce43-11ea-9f7f-67bb2533aee4.JPG)

마지막으로 `OnMessageActivityAsync` method의 마지막 부분에 위의 코드를 추가하여 Dialog을 실행시킬 수 있도록 합니다. `Dialog.RunAsync`는 Dialog를 실행시킬 수 있게 하는 method입니다.

위와 같이 설정하면 아무런 기능이 실행 중이지 않으면 **사용자의 입력이 들어올때마다 현재 봇의 문맥(상황)에 맞는 Dialog를 실행시킬 수 있게 모드를 설정할 수 있습니다**. 기능이 실행 중이라면 **진행중이던 Dialog를 계속 진행합니다**. 

Dialog는 기능/시나리오 라고 생각하면 편합니다. Dialog는 여러개가 있는데, 이 Dialog들마다 step이 존재하여 특정 시점에는 특정 Dialog의 특정 step이 실행되게 됩니다. 위의 함수는 특정 시점에서 진행중이던 Dialog의 순서를 이어서 진행시킨다 정도로 생각하면 될 것 같습니다.

Dialog는 순서를 원하는대로 설정 할 수 있어서 편하고, Dialog간 이동이 자유롭다는 장점이 있습니다. 하지만 WaterfallDialog 특성상 Dialog 내에서 이전 단계(step)으로 돌아갈 때 리스크가 있다는 단점이 있습니다.

### 2. Dialogs

Dialog는 봇의 기능과 같은 역할을 합니다. 즉, Dialog들은 내부에 저마다 개별적으로 실행시킬 수 있는 step을 가지고 있는데, 이 step들을 `WaterfallDialog`를 이용하여 순차적으로 실행시켜 특정 기능을 구현하는 것이라고 생각하면 되겠습니다.

/*그림*/

예시) `Dialog A(step 1)->Dialog A(step 2)->Dialog A(step 3)->Dialog B(step 1)->Dialog B(step 2)->Dialog A(step 4)`와 같이 실행시킬 수 있습니다.

Healthee는 Dialog를 이용하여 모드에 따라 다른 기능을 실행할 수 있게, 조건을 확인하여 특정 기능을 수행할지 말지 결정합니다.

#### 2.1 `Dialogs/MainDialog.cs`

*Healthee 구동에 필요한 Dialog들을 모두 실행시키는 관리자 Dialog와 같은 역할을 합니다. 실제 Dialog 끼리의 계층은 없지만 구조상 가장 최상위 Dialog입니다. 여기서는 모드에 따라 다른 Dialog들을 직접 실행시킵니다.*

**실행 중인 Dialog 있는지 확인**

위에 `EchoBot.cs` 에서 현재 실행중인 Dialog가 없을때만(현재 수행중인 기능이 없을 때) 모드를 바꾼다고 했습니다.

![7](https://user-images.githubusercontent.com/41438361/88439088-3413ce80-ce45-11ea-8a71-557511ee836d.JPG)

`MainDialog.cs`에서는 이 `is_running_dialog`를 통해 현재 실행중인 Dialog가 있는지 없는지 값을 **설정**해줍니다.

**필요한 Dialog 추가**

![8](https://user-images.githubusercontent.com/41438361/88439370-01b6a100-ce46-11ea-9322-351291627e4b.JPG)

Dialog도 실행시키기 전에 필요한 요소들을 **생성자에서** 세팅해야 합니다. 이 요소들은 

1. Prompt(사용자에게 입력을 촉구하는 부분)
2. 다른 Dialog(Dialog 내에서 직접 다른 Dialog를 실행시킬 때)

과 같습니다. `MainDialog.cs` 는 최상위 Dialog라고 할 수 있는데, 다른 모든 Dialog들을 `MainDialog.cs`안에서 직접 실행시킵니다. 따라서 필요한 Dialog들을 모두 `AddDialog(new DialogName())`으로 불러왔습니다. 상위 Dialog에서 하위 Dialog들을 실행시키기 위해서는 꼭 `AddDialog()` 를 통해 하위 Dialog를 추가시켜줘야 합니다.

맨 마지막에 있는 WaterfallDialog는 `{ }`안에 `InitialAsync`, `FinalStepAsync` 처럼 실행시키고 싶은 step(단계)를 추가해 줍니다. WaterfallDialog에 있는 step들은 위에서부터 순서대로 실행됩니다. 마지막으로 `InitialDialogId = nameof(WaterfallDialog)`를 통해 처음에 WaterfallDialog를 실행시키게 합니다.

**Dialog 실행**

![9](https://user-images.githubusercontent.com/41438361/88439527-6c67dc80-ce46-11ea-8a7d-7572cb7d0b39.JPG)

WaterFallDialog의 가장 첫 step인 `InitialDialog`는 위와 같습니다. 보면 `ModeManager.mode`를 참조하여 모드에 따라서 실행시킬 Dialog를 구분하여 실행시킵니다. 또 `MainDialog`를 포함해서 다른 Dialog들을 실행시키게 되는 부분이므로 현재 Dialog가 실행중이라는 `is_running_dialog`를 1로 설정합니다.

위의 `return await stepContext.BeginDialogAsync(nameof(Dialog이름), null, cancellationToken);` 함수는 특정 Dialog에서 바로 다른 Dialog로 이동할 수 있게 해주는 method입니다. 이 method를 통해 다른 Dialog가 실행이 되고 다른 Dialog가 종료된다면 이 Dialog의 다음 step이 실행되게 됩니다.

예를 들어 DialogA에 step1, step2, step3가 있고 step2에서 DialogB를 실행시켰다면 DialogB가 종료되었을 때 다시 DialogA의 step2로 돌아오는 것이 아닌 step3로 돌아가게 됩니다.

![10](https://user-images.githubusercontent.com/41438361/88439661-c2d51b00-ce46-11ea-9080-2bf228f86ce4.JPG)

마지막 step인 FinalstepAsync에서는 `is_running_dialog`를 0으로 설정하여 Dialog가 끝났음을 알립니다.

모든 Dialog에는 위처럼 `return await stepContext.EndDialogAsync(null, cancellationToken);` 가 포함이 되어야 합니다. 이 method는 Dialog를 종료시키는 역할을 합니다. 만약 WaterfallDialog의 중간 step에서 `EndDialogAsync` method가 실행되었다면 뒤에 얼마나 많은 step이 남았는지는  바로 해당 Dialog는 종료됩니다.

#### 2.2 `Dialogs/CheckUserDialog.cs`

![11](https://user-images.githubusercontent.com/41438361/88439858-51e23300-ce47-11ea-8da9-8e4ca5752cbc.JPG)

(윗 부분은 `DialogAndWelocomeBot.cs`의 `OnMembersAddedAsync` method에서 선언된 ) Healthee는 처음에 모드를 InitialCheckUser 모드로 설정하여 `CheckUserDialog.cs`가 실행될 수 있도록 했습니다. 

정리하면, 설정된 모드로 1. `EchoBot.cs`에서 `Dialog.RunAsync()` method가 실행되면서 현재 수행중이던 Dialog, 혹은 새로 Dialog를 시작하는데 이때 새로 기능을 실행시킬 경우(`MainDialog.is_running_dialog`가 0일 경우) 먼저 2. `MainDialog.cs`가 실행되고, 3. 모드 확인 후 모드에 맞는 Dialog가 실행될 수 있도록 되는 구조입니다.

사용자가 Healthee에서 처음으로 실행시키는 하위 Dialog인 `CheckUserDialog`는 사용자가 이전에 Healthee를 이용했는지 확인하고, 사용자가 Healthee를 처음 만났다면 사용자의 정보를 받는 `TutorialDialog`를 실행하고 이전에 이용했다면 바로 Healthee의 기능들을 카드로 나열한 것을 보여주는 것을 실행시키는 `ShowFunctionsDialog`를 실행시킵니다.

![12](https://user-images.githubusercontent.com/41438361/88440318-99b58a00-ce48-11ea-8d4a-bc86f4f2eed3.JPG)

`MainDialog.cs`와 마찬가지로 생성자 부분에 필요한 Dialog를 추가하고 WaterfallDialog를 구현합니다. 이 Dialog의 step에서 `TutorialDialog.cs`를 실행시키므로 `AddDialog()` method를 이용해서 추가해줍니다.

`TextPrompt`, `ChoicePrompt`같은 것은 step에서 다음 step으로 넘어갈 때 사용자가 입력해야 하는 포맷이 정해져 있을 때(`TextPrompt`일 경우 사용자는 다음 step으로 넘어가기 위해 텍스트를 입력해야 하고 `ChoicePrompt`일 경우 나열되는 선택지 중에 하나를 선택해야 합니다.) 필요한 프롬프트인데, WaterfallDialog의 각 step에서 사용할 수 있습니다. 이것들도 하위 Dialog와 마찬가지로 `AddDialog()` method를 이용해서 추가시켜야 정상적으로 이용할 수 있습니다.

![13](https://user-images.githubusercontent.com/41438361/88440520-24968480-ce49-11ea-9e8d-361e517f658a.JPG)

예를 들어, 이 Dialog에서 가장 처음에 실행되는 `AskVisitedAsync` 단계에서는 다음 단계로 넘어가기 위해 `ChoicePrompt`를 이용하여 사용자에게 "응", "아니" 중 하나를 선택하도록 만들었습니다. 만약 사용자가 이 외의 값을 입력할 경우 둘 중 하나의 값을 무조건 입력할 때까지 이 step이 반복됩니다.

![14](https://user-images.githubusercontent.com/41438361/88440606-70e1c480-ce49-11ea-928f-cbcce3739287.JPG)

다음에 실행되는 step인 `AskNameAsync`에서는 `((FoundChoice)stepContext.Result).Value.Trim()`를 이용해서 이전 단계에서 사용자가 입력한(선택한) 값을 가져옵니다. 이처럼 항상 사용자에게 입력값을 받으면 다음 step에서 받아올 수 있습니다.

만약 "응"을 선택했을 경우, `TextPrompt`를 이용해서 사용자에게 이름을 입력하도록 만들었고, "아니"를 선택했을 경우 `stepContext.BeginDialogAsync(nameof(TutorialDialog), null, cancellationToken);`를 이용하여 `TutorialDialog.cs`를 실행시킵니다.

`stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 1;`은 WaterFallDialog에서 특정 step을 실행시키고 싶을 때 이용할 수 있습니다. 만약 마지막에 +1이 아닌 0을 했다면 바로 다음 단계가 실행됩니다. Healthee는 +1를 하여 WaterfallDialog에서 한 step을 건너뛸 수 있도록 했습니다. 만약 이렇게 설정하지 않는다면, WaterfallDialog에 있는 step들은 무조건 차례대로 실행되기 때문에 바로 다음 step이 실행됩니다. 이 코드에서는 `TutorialDialog.cs`를 마치고 다시 이 Dialog로 돌아왔을 때 한 step을 건너뛰겠다라는 말이 됩니다.

![15](https://user-images.githubusercontent.com/41438361/88440862-30367b00-ce4a-11ea-96db-dfae67146f51.JPG)

다음 step인 `CheckValidUserAsync` 에서는 사용자의 이름을 가지고 Azure sql 에 있는 데이터베이스를 검색해 사용자 이름 정보가 `UserInfo` 테이블에 있는지 검사합니다. Azure sql과 Bot을 연결하는 부분도 후에 추가로 밑에 설명을 달아놓겠습니다.

쿼리들을 이용하여 해당 username이 데이터베이스에 없으면 아래와 같이 `TutorialDialog.cs`를 실행시킵니다.

![16](https://user-images.githubusercontent.com/41438361/88441063-dda98e80-ce4a-11ea-9b9a-b1cc20bccfe1.JPG)

만약 데이터베이스에 있다면 아래와 같이 `UserInfoManager`에 정보들을 저장하고 `ShowFunctionDialog`를 실행시킵니다. 

![17](https://user-images.githubusercontent.com/41438361/88441088-f619a900-ce4a-11ea-8f6d-4e298bdf0ea3.JPG)


#### 2.3 `Dialogs/TutorialDialog.cs`

![18](https://user-images.githubusercontent.com/41438361/88441179-3f69f880-ce4b-11ea-860a-f0e45aa59262.JPG)

`TutorialDialog.cs`에서 실행시키는 Step들은 위와 같습니다. step의 주석에서도 볼 수 있듯이 `TutorialDialog`에서는 사용자 정보를 받아 저장하고, 데이터베이스에 넣는 역할을 합니다.

`AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), WeightPromptValidatorAsync));`는 `NumberPrompt`를 실행시킬 때 `WeightPromptValidatorAsync`를 유효성 검사기로 사용하겠다는 뜻입니다.

![20](https://user-images.githubusercontent.com/41438361/88441359-d2a32e00-ce4b-11ea-8e12-ba5e43ef375e.JPG)

이 Dialog에서 실행되는 `PreWeightStepAsync` 단계에서는 사용자에게 현재 체중을 `NumberPrompt<int>`를 이용하여 int 값으로 입력하게 만듭니다. 사용자가 숫자를 입력하면, 

![21](https://user-images.githubusercontent.com/41438361/88441444-1eee6e00-ce4c-11ea-940b-37233c3b764a.JPG)

위의 `PreWeightStepAsync`가 실행되어 입력한 값을 검사합니다. 만약 `>0`조건을 만족시킬 경우 다음 step으로 정상적으로 넘어가고, 만약 조건을 만족하지 않았을 경우 `RetryPrompt`가 출력되며 사용자에게 다시 입력할 것을 봇이 얘기하게 됩니다. 이처럼 특정 prompt에서 조건을 확인할 수 있는 함수를 생성할 수 있습니다.

![19](https://user-images.githubusercontent.com/41438361/88441267-83f59400-ce4b-11ea-8e80-782201c8c8d8.JPG)

후에 실행되는 `AcknowledgementStepAsync` 단계에서는 위와 같이 INSERT 쿼리문을 이용하여 데이터베이스에 값을 삽입합니다. 

#### 2.4 `Dialogs/ShowFunctionsDialog.cs`

`ShowFunctionDialog`에서는 Healthee가 제공하는 기능들을 카드로 보여줍니다.

![22](https://user-images.githubusercontent.com/41438361/88441622-93c1a800-ce4c-11ea-9705-65b52bb37ea6.JPG)

카드를 보여주는 step인 `ShowCardStepAsync`에서는 위와 같이 `attachments`에 `HeroCard` 객체를 생성하여 붙입니다. `HeroCard` 안에는 다양한 필드를 이용하여 다양한 카드를 만들 수 있습니다. 여기서의 `ImBack`은 사용자가 `Button`을 누르면 `value`의 값을 봇한테 전송하는 것을 가능하게 하는 ActionType입니다.

여기서는 `reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;`를 이용하여 카드가 회전식으로 보여지게 합니다.

봇에서는 다음과 같이 보입니다.

![23](https://user-images.githubusercontent.com/41438361/88441781-2eba8200-ce4d-11ea-813e-d2ad41fff170.JPG)

오른쪽에 마우스를 갖다대면 화살표가 나와 클릭하면 카드를 이동시키며 볼 수 있습니다.

#### 2.5 `Dialogs/RecommendExerciseDialog.cs`

![24](https://user-images.githubusercontent.com/41438361/88441859-6aede280-ce4d-11ea-9fa7-139b55513a0a.JPG)

`RecommendExerciseDialog`에서 실행되는 step들은 위와 같습니다. 이 dialog에서는 사용자가 초기에 설정한 운동 부위, 종류, 난이도에 맞는 운동을 바로 추천받을 수도 있고, 사용자가 추가로 해당 정보들을 설정하여 운동을 새로 추천받을 수도 있습니다. 운동을 추천해줄때는 운동 이름, 세트, 시간, 영상까지 함께 `HeroCard`로 보여줍니다.

마지막에는 추천 받은 운동을 했는지 확인하여 만약 운동을 했다면 데이터베이스에 사용자의 운동기록을 업데이트합니다.

마찬가지로 prompt와 db query를 이용하여 기능들을 구현했습니다.

#### 2.6 `Dialogs/RecordDialog.cs`

![25](https://user-images.githubusercontent.com/41438361/88442024-ed76a200-ce4d-11ea-9adb-7445a26db6ea.JPG)

`RecordDialog`에서 실행되는 step들은 위와 같습니다. 이 dialog에서는 사용자가 운동한 부위와 시간을 확인하여 사용자 정보를 업데이트 시킵니다.

#### 2.7 `Dialogs/RecommendFood.cs`

![26](https://user-images.githubusercontent.com/41438361/88442254-c9679080-ce4e-11ea-8e12-93f57483a3da.JPG)

`RecommendFood`에서 실행되는 step들은 위와 같습니다. 이 dialog에서는 데이터베이스의 음식 테이블을 조회해 랜덤으로 음식을 하나 뽑아 추천해줍니다. 음식을 추천해줄때는 이름, 사진, 설명까지 함께 `HeroCard`로 보여줍니다.

#### 2.8 `Dialogs/RecommendEquipment.cs`

![27](https://user-images.githubusercontent.com/41438361/88442288-e9974f80-ce4e-11ea-9e88-1efa1d093ef1.JPG)

`RecommendEquipment`에서 실행되는 step들은 위와 같습니다. 이 dialog에서는 데이터베이스의 운동 기구 테이블을 조회한 다음 사용자가 설정한 운동 부위에 맞는 운동 기구를 추천해줍니다. 운동 기구를 추천해줄때는 판매처, 영상까지 함께 `HeroCard`로 보여줍니다.

#### 2.9 `Dialogs/SeeMyCharacterDialog.cs`

![28](https://user-images.githubusercontent.com/41438361/88442433-72ae8680-ce4f-11ea-97cb-7734e92c6ec0.JPG)

`SeeMyCharacterDialog`에서 실행되는 step들은 위와 같습니다. 이 dialog에서는 사용자의 정보를 조회하여 사용자의 운동 기록 횟수를 통해 캐릭터가 어떤 상태인지 판단합니다. 캐릭터의 상태에 따라 다른 이미지와 문구를 사용자에게 `HeroCard`로 보여줍니다.

#### 2.10 `Dialogs/SeeMyCharacterDialog.cs`

![29](https://user-images.githubusercontent.com/41438361/88442505-b903e580-ce4f-11ea-956c-e9c46a092283.JPG)

`Dialogs/SeeMyCharacterDialog`에서 실행되는 step들은 위와 같습니다. 이 dialog에서는 사용자의 정보를 조회하여 부위별 운동 시간을 `ReceiptCard`로 보여줍니다.

![30](https://user-images.githubusercontent.com/41438361/88442566-f49eaf80-ce4f-11ea-8f56-13720ce6adfc.JPG)

출력 화면은 위와 같습니다.

### 3. Resources

여기에 있는 json파일들은 모두 `AdaptiveCard`로 변환될 수 있는 양식입니다.

직접 일일이 만들지 않아도 되는데, [AdaptiveCard 쉽게 만들기](https://adaptivecards.io/designer/)에서 쉽게 UI로 만들 수 있습니다.

### 4.`Cards.cs`

![31](https://user-images.githubusercontent.com/41438361/88442694-6f67ca80-ce50-11ea-8046-754524a66c84.JPG)

`CreateAdaptiveCardAttachment` method를 이용해서 `cardName`을 "~.json" 형식으로 적어주면 해당 json 파일을 adaptive card로 변환할 수 있습니다.

### 5. `ModeManager.cs`

![32](https://user-images.githubusercontent.com/41438361/88442761-af2eb200-ce50-11ea-91ee-eea8ff0c3a85.JPG)

`ModeManager` 에서는 모드(Healthee가 지원하는 기능별로 존재)를 선언합니다. 이 모드를 다른 코드에서 바꿔가며 모드를 조작해줍니다.

### 6. `UserInfoManager.cs`

![34](https://user-images.githubusercontent.com/41438361/88442864-09c80e00-ce51-11ea-8fc6-ccebb59e1308.JPG)

`UserInfoManager`에서는 사용자 정보를 저장하기 위한 변수를 선언합니다.

### 7. `Startup.cs`

![35](https://user-images.githubusercontent.com/41438361/88442923-354af880-ce51-11ea-96e6-5aa2c17ff0e2.JPG)

`Startup.cs`에서는 필요한 설정들과 Dialog를 추가하여 Bot을 생성합니다. 여기서 `services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();` 때문에 기본적으로 `MainDialog`가 실행이 되는 것입니다.

### 8. `appsettings.json`

![36](https://user-images.githubusercontent.com/41438361/88443003-91158180-ce51-11ea-8763-883c454e2355.JPG)

`appsettings.json`에서는 위와 같이 필요한 key와 value를 설정합니다.


## 주의 사항

### 모듈

추가적으로 모듈을 설치할 필요는 없습니다. 현재 Healthee에서 사용하고 있는 NuGet Package는 다음과 같습니다.

* AdaptiveCards
* AutoMapper
* EntityFramework
* Microsoft.AspNetCore.Mvc.NewtonsoftJson
* Microsoft.Bot.Builder
* Microsoft.Bot.Builder.AI.QnA
* Microsoft.Bot.Builder.Dialogs
* Microsoft.Bot.Builder.Integration.AspNet.Core
* Microsoft.Recognizers.Text
* Microsoft.Recognizers.Text.DateTime
* Microsoft.Recognizers.Text.Number

### 데이터베이스 관련(방화벽)

봇을 C# project로 다운받아 로컬에서 실행시켜 emulator로 확인할 때, 간혹 쿼리문과 연결 설정이 올바르게 되어 있는데도 연결 오류가 뜨는 경우가 있었습니다. 

이 경우 방화벽 문제였는데, 

![1](https://user-images.githubusercontent.com/41438361/88452230-fc824200-ce97-11ea-989c-72436e7fd200.PNG)

Azure Portal > Sql Server > 해당 데이터베이스 > 쿼리 편집기 에서 위와 같이 로그인을 할 때 IP address와 함께 방화벽 문제로 연결이 되지 않는다는 메세지가 떴습니다.

현재 Healthee의 sql 데이터베이스 설정은 아래와 같습니다.

![37](https://user-images.githubusercontent.com/41438361/88443331-d25a6100-ce52-11ea-967c-0156d9faf43c.JPG)

설정을 위와 같이 해놓았지만 항상 새로운 IP에서 접속하기 위해서는 해당 IP주소를 portal을 이용해서 추가해 주어야 했습니다.

![38](https://user-images.githubusercontent.com/41438361/88443389-18afc000-ce53-11ea-9183-fd2077a7355d.JPG)

만약 Chatbot을 구동중 DB 연결 오류가 계속 뜰 시에는 Azure Portal > 해당 **sql server**(데이터베이스가 아닙니다) > 보안 > 방화벽 및 가상 네트워크에서 위에 있는 클라이언트 IP 주소를 시작 IP,  종료 IP에 넣어주고 추가한다음, 위에 있는 저장 버튼을 누르고 다시 접속해주세요. (규칙이름은 아무 값이나 넣어도 됩니다.) 이렇게 해도 안 될 경우 이 *데이터베이스 관련* 부분의 처음 사진에 있는 형광펜으로 친 IP 주소를 추가해주세요.


## 데이터베이스

### 데이터베이스 설명


### AZURE PORTAL에서 데이터베이스 쿼리문 이용하여 데이터 조작하기

![2](https://user-images.githubusercontent.com/41438361/88452797-a794fa80-ce9c-11ea-805b-cc7129acb827.PNG)

AZURE Portal에 접속한 후, 리소스 그룹에서 SQL 데이터베이스(혹은 SQL 서버 > SQL 데이터베이스)에 접속합니다.

형광펜을 친 곳에 암호를 기입하고 확인을 눌러 쿼리 편집기를 시작합니다.

![3](https://user-images.githubusercontent.com/41438361/88452857-078ba100-ce9d-11ea-88b1-86c34ddb516b.PNG)

쿼리 편집기로 넘어가면, 위와 같이 왼쪽에서는 테이블들과 필드, 그리고 오른쪽에는 직접 쿼리문을 쳐서 실행시킬 수 있는 편집기가 나옵니다.

1. 데이터 조회 SELECT

2. 데이터 업데이트 UPDATE

3. 테이블 혹은 데이터베이스 삭제 DROP

4. 데이터 삽입 INSERT


## 그 외 알아낸 소소한 것들

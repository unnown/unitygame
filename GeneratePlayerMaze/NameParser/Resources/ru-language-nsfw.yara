rule content_ru_language_nsfw {
  strings:
      $  =  "bychara"  fullword wide ascii nocase
      $  =  "byk"  fullword wide ascii nocase
      $  =  "chernozhopyi"  fullword wide ascii nocase
      $  =  "dolboy'eb"  fullword wide ascii nocase
      $  =  "ebalnik"  fullword wide ascii nocase
      $  =  "ebalo"  fullword wide ascii nocase
      $  =  "ebalom sch'elkat"  fullword wide ascii nocase
      $  =  "gol"  fullword wide ascii nocase
      $  =  "mudack"  fullword wide ascii nocase
      $  =  "opizdenet"  fullword wide ascii nocase
      $  =  "osto'eblo"  fullword wide ascii nocase
      $  =  "ostokhuitel'no"  fullword wide ascii nocase
      $  =  "ot'ebis"  fullword wide ascii nocase
      $  =  "otmudohat"  fullword wide ascii nocase
      $  =  "otpizdit"  fullword wide ascii nocase
      $  =  "otsosi"  fullword wide ascii nocase
      $  =  "padlo"  fullword wide ascii nocase
      $  =  "pedik"  fullword wide ascii nocase
      $  =  "perdet"  fullword wide ascii nocase
      $  =  "petuh"  fullword wide ascii nocase
      $  =  "pidar gnoinyj"  fullword wide ascii nocase
      $  =  "piz'det"  fullword wide ascii nocase
      $  =  "piz`dyulina"  fullword wide ascii nocase
      $  =  "pizd'uk"  fullword wide ascii nocase
      $  =  "pizda"  fullword wide ascii nocase
      $  =  "pizdato"  fullword wide ascii nocase
      $  =  "pizdatyi"  fullword wide ascii nocase
      $  =  "pizdetc"  fullword wide ascii nocase
      $  =  "pizdoi nakryt'sja"  fullword wide ascii nocase
      $  =  "po khuy"  fullword wide ascii nocase
      $  =  "po'imat' na konchik"  fullword wide ascii nocase
      $  =  "po'iti posrat"  fullword wide ascii nocase
      $  =  "podi ku'evo"  fullword wide ascii nocase
      $  =  "poeben"  fullword wide ascii nocase
      $  =  "poluchit pizdy"  fullword wide ascii nocase
      $  =  "pososi moyu konfetku"  fullword wide ascii nocase
      $  =  "prissat"  fullword wide ascii nocase
      $  =  "proebat"  fullword wide ascii nocase
      $  =  "promudobl'adsksya pizdopro'ebina"  fullword wide ascii nocase
      $  =  "propezdoloch"  fullword wide ascii nocase
      $  =  "prosrat"  fullword wide ascii nocase
      $  =  "raspeezdeyi"  fullword wide ascii nocase
      $  =  "raspizdatyi"  fullword wide ascii nocase
      $  =  "raz'yebuy"  fullword wide ascii nocase
      $  =  "raz'yoba"  fullword wide ascii nocase
      $  =  "s'ebat'sya"  fullword wide ascii nocase
      $  =  "shalava"  fullword wide ascii nocase
      $  =  "styervo"  fullword wide ascii nocase
      $  =  "sukin syn"  fullword wide ascii nocase
      $  =  "svodit posrat"  fullword wide ascii nocase
      $  =  "svoloch"  fullword wide ascii nocase
      $  =  "trakhat'sya"  fullword wide ascii nocase
      $  =  "trimandoblydskiy pizdoproyob"  fullword wide ascii nocase
      $  =  "u'ebitsche"  fullword wide ascii nocase
      $  =  "ubl'yudok"  fullword wide ascii nocase
      $  =  "uboy"  fullword wide ascii nocase
      $  =  "v pizdu"  fullword wide ascii nocase
      $  =  "vafl'a"  fullword wide ascii nocase
      $  =  "vafli lovit"  fullword wide ascii nocase
      $  =  "vyperdysh"  fullword wide ascii nocase
      $  =  "vzdrochennyi"  fullword wide ascii nocase
      $  =  "yeb vas"  fullword wide ascii nocase
      $  =  "za'ebat"  fullword wide ascii nocase
      $  =  "zaebis"  fullword wide ascii nocase
      $  =  "zalupa"  fullword wide ascii nocase
      $  =  "zalupat"  fullword wide ascii nocase
      $  =  "zasranetc"  fullword wide ascii nocase
      $  =  "zassat"  fullword wide ascii nocase
      $  =  "zlo'ebuchy"  fullword wide ascii nocase
      $  =  "ни за хуй собачу"  fullword wide ascii nocase
      $  =  "на фиг"  fullword wide ascii nocase
      $  =  "на хуй"  fullword wide ascii nocase
      $  =  "на хуя"  fullword wide ascii nocase
      $  =  "ни хуя"  fullword wide ascii nocase
      $  =  "ты мне ваньку не валяй"  fullword wide ascii nocase
      $  =  "на хую вертеть"  fullword wide ascii nocase
      $  =  "не ебет"  fullword wide ascii nocase
      $  =  "ёб твою мать"  fullword wide ascii nocase
      $  =  "во пизду"  fullword wide ascii nocase
      $  =  "хуй"  fullword wide ascii nocase
      $  =  "хер с ней"  fullword wide ascii nocase
      $  =  "хер с ним"  fullword wide ascii nocase
      $  =  "как два пальца обоссать"  fullword wide ascii nocase
      $  =  "хуй пинать"  fullword wide ascii nocase
      $  =  "траxать"  fullword wide ascii nocase
      $  =  "ебло"  fullword wide ascii nocase
      $  =  "жопа"  fullword wide ascii nocase
      $  =  "мент"  fullword wide ascii nocase
      $  =  "муда"  fullword wide ascii nocase
      $  =  "секс"  fullword wide ascii nocase
      $  =  "фига"  fullword wide ascii nocase
      $  =  "хрен"  fullword wide ascii nocase
      $  =  "дать пизды"  fullword wide ascii nocase
      $  =  "хуем груши околачивать"  fullword wide ascii nocase
      $  =  "один ебётся"  fullword wide ascii nocase
      $  =  "блядь"  fullword wide ascii nocase
      $  =  "бугор"  fullword wide ascii nocase
      $  =  "говно"  fullword wide ascii nocase
      $  =  "голый"  fullword wide ascii nocase
      $  =  "ебать"  fullword wide ascii nocase
      $  =  "манда"  fullword wide ascii nocase
      $  =  "сиски"  fullword wide ascii nocase
      $  =  "срать"  fullword wide ascii nocase
      $  =  "ссать"  fullword wide ascii nocase
      $  =  "хохол"  fullword wide ascii nocase
      $  =  "хуило"  fullword wide ascii nocase
      $  =  "хуиня"  fullword wide ascii nocase
      $  =  "хуёво"  fullword wide ascii nocase
      $  =  "ёбарь"  fullword wide ascii nocase
      $  =  "ебать-копать"  fullword wide ascii nocase
      $  =  "бардак"  fullword wide ascii nocase
      $  =  "блядки"  fullword wide ascii nocase
      $  =  "гандон"  fullword wide ascii nocase
      $  =  "говнюк"  fullword wide ascii nocase
      $  =  "дерьмо"  fullword wide ascii nocase
      $  =  "ебнуть"  fullword wide ascii nocase
      $  =  "малофя"  fullword wide ascii nocase
      $  =  "мудило"  fullword wide ascii nocase
      $  =  "охуеть"  fullword wide ascii nocase
      $  =  "хапать"  fullword wide ascii nocase
      $  =  "хуёвый"  fullword wide ascii nocase
      $  =  "лысого в кулаке гонять"  fullword wide ascii nocase
      $  =  "играть на кожаной флейте"  fullword wide ascii nocase
      $  =  "курите мою трубку"  fullword wide ascii nocase
      $  =  "встать раком"  fullword wide ascii nocase
      $  =  "каждый дрочит как он хочет"  fullword wide ascii nocase
      $  =  "хуиней страдать"  fullword wide ascii nocase
      $  =  "другой дразнится"  fullword wide ascii nocase
      $  =  "бздёнок"  fullword wide ascii nocase
      $  =  "дрочить"  fullword wide ascii nocase
      $  =  "жополиз"  fullword wide ascii nocase
      $  =  "наебать"  fullword wide ascii nocase
      $  =  "опесдол"  fullword wide ascii nocase
      $  =  "офигеть"  fullword wide ascii nocase
      $  =  "хуеплет"  fullword wide ascii nocase
      $  =  "хуйнуть"  fullword wide ascii nocase
      $  =  "половое сношение"  fullword wide ascii nocase
      $  =  "блядство"  fullword wide ascii nocase
      $  =  "мудозмон"  fullword wide ascii nocase
      $  =  "спиздить"  fullword wide ascii nocase
      $  =  "блядовать"  fullword wide ascii nocase
      $  =  "измудохать"  fullword wide ascii nocase
      $  =  "мандавошка"  fullword wide ascii nocase
      $  =  "наебнуться"  fullword wide ascii nocase
      $  =  "невебенный"  fullword wide ascii nocase
      $  =  "обнаженный"  fullword wide ascii nocase
      $  =  "охуйтельно"  fullword wide ascii nocase
      $  =  "обоссаться можно"  fullword wide ascii nocase
      $  =  "выёбываться"  fullword wide ascii nocase
      $  =  "наебениться"  fullword wide ascii nocase
      $  =  "нахуячиться"  fullword wide ascii nocase
  condition:
    1 of them
}

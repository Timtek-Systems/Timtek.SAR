using Timtek.SAR.Domain.ValueObjects;

namespace Timtek.SAR.Tests.Domain.ValueObjects;

[Subject("What3Words Address")]
class When_creating_a_what3words_address_with_three_valid_words
{
    static What3WordsAddress _result;

    Because of = () => _result = new What3WordsAddress("filled", "count", "soap");

    It should_store_word_one = () => _result.Word1.ShouldEqual("filled");
    It should_store_word_two = () => _result.Word2.ShouldEqual("count");
    It should_store_word_three = () => _result.Word3.ShouldEqual("soap");
}

[Subject("What3Words Address")]
class When_displaying_a_what3words_address
{
    static What3WordsAddress _address;
    static string _display;

    Establish context = () => _address = new What3WordsAddress("filled", "count", "soap");

    Because of = () => _display = _address.ToString();

    It should_use_the_triple_slash_prefix = () => _display.ShouldStartWith("///");
    It should_separate_words_with_dots = () => _display.ShouldEqual("///filled.count.soap");
}

[Subject("What3Words Address")]
class When_creating_a_what3words_address_with_a_null_word
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new What3WordsAddress(null!, "count", "soap"));

    It should_throw_an_argument_null_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentNullException>();
}

[Subject("What3Words Address")]
class When_creating_a_what3words_address_with_an_empty_word
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new What3WordsAddress("filled", "", "soap"));

    It should_throw_an_argument_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentException>();
}

[Subject("What3Words Address")]
class When_creating_a_what3words_address_with_a_whitespace_word
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => new What3WordsAddress("filled", "count", "  "));

    It should_throw_an_argument_exception = () =>
        _exception.ShouldBeOfExactType<ArgumentException>();
}

[Subject("What3Words Address")]
class When_comparing_two_what3words_addresses_with_the_same_words
{
    static What3WordsAddress _a;
    static What3WordsAddress _b;

    Establish context = () =>
    {
        _a = new What3WordsAddress("filled", "count", "soap");
        _b = new What3WordsAddress("filled", "count", "soap");
    };

    It should_be_equal = () => _a.ShouldEqual(_b);
    It should_have_the_same_hash_code = () => _a.GetHashCode().ShouldEqual(_b.GetHashCode());
}

[Subject("What3Words Address")]
class When_comparing_two_what3words_addresses_with_different_words
{
    static What3WordsAddress _a;
    static What3WordsAddress _b;

    Establish context = () =>
    {
        _a = new What3WordsAddress("filled", "count", "soap");
        _b = new What3WordsAddress("index", "home", "raft");
    };

    It should_not_be_equal = () => _a.ShouldNotEqual(_b);
}

[Subject("What3Words Address")]
class When_parsing_a_what3words_string_with_prefix
{
    static What3WordsAddress _result;

    Because of = () => _result = What3WordsAddress.Parse("///filled.count.soap");

    It should_extract_word_one = () => _result.Word1.ShouldEqual("filled");
    It should_extract_word_two = () => _result.Word2.ShouldEqual("count");
    It should_extract_word_three = () => _result.Word3.ShouldEqual("soap");
}

[Subject("What3Words Address")]
class When_parsing_a_what3words_string_without_prefix
{
    static What3WordsAddress _result;

    Because of = () => _result = What3WordsAddress.Parse("filled.count.soap");

    It should_extract_word_one = () => _result.Word1.ShouldEqual("filled");
    It should_extract_word_two = () => _result.Word2.ShouldEqual("count");
    It should_extract_word_three = () => _result.Word3.ShouldEqual("soap");
}

[Subject("What3Words Address")]
class When_parsing_an_invalid_what3words_string
{
    static Exception _exception;

    Because of = () => _exception = Catch.Exception(() => What3WordsAddress.Parse("only.two"));

    It should_throw_a_format_exception = () =>
        _exception.ShouldBeOfExactType<FormatException>();
}
